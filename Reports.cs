using Microsoft.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace kpz_lr3.Reports
{
    public partial class Reports : Form
    {
        private readonly string connectionString;
        private bool reportReady;

        public Reports() : this("Server=localhost;Database=My db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;Connect Timeout=10;") { }
        public Reports(string connection)
        {
            InitializeComponent();
            connectionString = connection;
            cmbKind.Items.AddRange(new object[] {
                "Каталог тренувань + діаграма за метою",
                "Заявки всіх користувачів + діаграма за метою",
                "Заявки одного користувача + діаграма інтенсивності"
            });
            cmbKind.SelectedIndexChanged += OptionsChanged;
            cmbUser.SelectedIndexChanged += OptionsChanged;
            chkPeriod.CheckedChanged += OptionsChanged;
            dateFrom.ValueChanged += OptionsChanged;
            dateTo.ValueChanged += OptionsChanged;
            btnBuild.Click += BuildClick;
            btnPdf.Click += PdfClick;
            btnClose.Click += CloseClick;
            Shown += ReportsShown;
            cmbKind.SelectedIndex = 0;
        }
        private void ReportsShown(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var adapter = new SqlDataAdapter("SELECT Id, FullName FROM dbo.Users ORDER BY FullName, Id;", connection))
                {
                    var users = new DataTable();
                    adapter.Fill(users);
                    cmbUser.DisplayMember = "FullName";
                    cmbUser.ValueMember = "Id";
                    cmbUser.DataSource = users;
                }
               
            }
            catch (Exception ex) { ShowError(ex); }
        }
        private void OptionsChanged(object sender, EventArgs e)
        {
            cmbUser.Enabled = cmbKind.SelectedIndex == 2;
            chkPeriod.Enabled = cmbKind.SelectedIndex > 0;
            dateFrom.Enabled = dateTo.Enabled = chkPeriod.Enabled && chkPeriod.Checked;
            reportReady = false;
            btnPdf.Enabled = false;
            viewer.Clear();
            
        }
        private void CloseClick(object sender, EventArgs e) { Close(); }
        private void BuildClick(object sender, EventArgs e)
        {
            try { BuildReport(); } catch (Exception ex) { ShowError(ex); }
        }
        private void BuildReport()
        {
            reportReady = false;
            btnPdf.Enabled = false;
            viewer.Clear();
            bool catalog = cmbKind.SelectedIndex == 0;
            bool personal = cmbKind.SelectedIndex == 2;
            bool useDates = !catalog && chkPeriod.Checked;
            if (useDates && dateFrom.Value.Date > dateTo.Value.Date)
                throw new Exception("Початкова дата не може бути пізнішою за кінцеву.");
            if (personal && !(cmbUser.SelectedValue is int))
                throw new Exception("Оберіть користувача. Якщо список порожній — додайте його на головній формі.");

            string file = catalog ? "WorkoutsReport.rdlc" : "RequestsReport.rdlc";
            string path = Path.Combine(AppContext.BaseDirectory, "Reports", file);
            if (!File.Exists(path))
                throw new Exception("Файл звіту не знайдено: " + path + "\nПеревірте Copy to Output Directory = Copy if newer для RDLC-файлів.");

            string sql = catalog ? CatalogSql : RequestsSql;
            string chartTitle = "Кількість тренувань за метою";
            string reportTitle = "Каталог тренувань";
            string subtitle = "База: My db. У таблиці показано плани та кількість вправ у кожному.";
            if (!catalog)
            {
                if (personal)
                {
                  
                    sql = RequestsSql.Replace("g.Name AS Category", "i.Name AS Category")
                        .Replace("GROUP BY g.Id, g.Name", "GROUP BY i.Id, i.Name")
                        .Replace("ORDER BY g.Id;", "ORDER BY i.Id;");
                    chartTitle = "Кількість заявок за інтенсивністю";
                    reportTitle = "Історія заявок користувача";
                    subtitle = "Користувач: " + cmbUser.Text + ", ID: " + cmbUser.SelectedValue + ". ";
                }
                else
                {
                    chartTitle = "Кількість заявок за метою";
                    reportTitle = "Заявки всіх користувачів";
                    subtitle = "Усі користувачі. ";
                }
                subtitle += useDates ? "Період: " + dateFrom.Value.ToString("dd.MM.yyyy") + " — " + dateTo.Value.ToString("dd.MM.yyyy") : "Період: за весь час";
            }

            var data = new DataSet();
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                if (!catalog)
                {
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = personal ? (int)cmbUser.SelectedValue : 0;
                    command.Parameters.Add("@UseDates", SqlDbType.Bit).Value = useDates;
                    command.Parameters.Add("@DateFrom", SqlDbType.DateTime2).Value = dateFrom.Value.Date;
                    command.Parameters.Add("@DateToExclusive", SqlDbType.DateTime2).Value = dateTo.Value.Date.AddDays(1);
                }
                using (var adapter = new SqlDataAdapter(command)) adapter.Fill(data);
            }
            if (data.Tables.Count != 2) throw new Exception("SQL має повернути дві таблиці: основні дані та статистику.");
            DataTable mainData = data.Tables[0];
            DataTable statsData = data.Tables[1];
            data.Tables[0].TableName = "MainData";
            data.Tables[1].TableName = "StatsData";

            viewer.Reset();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = path;
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("MainData", mainData));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("StatsData", statsData));
            viewer.LocalReport.SetParameters(new[] {
                new ReportParameter("ReportTitle", reportTitle),
                new ReportParameter("ReportSubtitle", subtitle),
                new ReportParameter("ChartTitle", chartTitle)
            });
            viewer.RefreshReport();
            reportReady = true;
            btnPdf.Enabled = true;
           
        }
        private void PdfClick(object sender, EventArgs e)
        {
            if (!reportReady) return;
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PDF (*.pdf)|*.pdf";
                    dialog.FileName = "WorkoutReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    byte[] bytes = viewer.LocalReport.Render("PDF");
                    File.WriteAllBytes(dialog.FileName, bytes);
                    MessageBox.Show(this, "Звіт збережено:\n" + dialog.FileName, "PDF");
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }
        private void ShowError(Exception ex)
        {
            string message = ex.Message;
            Exception inner = ex.InnerException;
            while (inner != null) { message += "\n\n" + inner.Message; inner = inner.InnerException; }
            MessageBox.Show(this, message, "Помилка звіту", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       
        
        private const string CatalogSql = @"
SELECT w.Id, w.Name, g.Name AS Goal, i.Name AS Intensity,
       w.SessionsPerWeek, w.RequiredLevel,
       (SELECT COUNT(*) FROM dbo.WorkoutExercises we WHERE we.WorkoutId=w.Id) AS ExerciseCount
FROM dbo.Workouts w
JOIN dbo.Goals g ON g.Id=w.GoalId
JOIN dbo.Intensities i ON i.Id=w.IntensityId
ORDER BY g.Id, w.Name, w.Id;

SELECT g.Name AS Category, COUNT(w.Id) AS Total
FROM dbo.Goals g LEFT JOIN dbo.Workouts w ON w.GoalId=g.Id
GROUP BY g.Id, g.Name ORDER BY g.Id;";

       
        private const string RequestsSql = @"
SELECT r.Id, u.FullName, g.Name AS Goal, i.Name AS Intensity,
       r.SessionsPerWeek, r.ExperienceAtRequest, r.CreatedAt
FROM dbo.Requests r
JOIN dbo.Users u ON u.Id=r.UserId
JOIN dbo.Goals g ON g.Id=r.GoalId
JOIN dbo.Intensities i ON i.Id=r.IntensityId
WHERE (@UserId=0 OR r.UserId=@UserId)
  AND (@UseDates=0 OR (r.CreatedAt>=@DateFrom AND r.CreatedAt<@DateToExclusive))
ORDER BY r.CreatedAt DESC, r.Id DESC;

SELECT g.Name AS Category, COUNT(*) AS Total
FROM dbo.Requests r
JOIN dbo.Goals g ON g.Id=r.GoalId
JOIN dbo.Intensities i ON i.Id=r.IntensityId
WHERE (@UserId=0 OR r.UserId=@UserId)
  AND (@UseDates=0 OR (r.CreatedAt>=@DateFrom AND r.CreatedAt<@DateToExclusive))
GROUP BY g.Id, g.Name ORDER BY g.Id;";
    }
}
