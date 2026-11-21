using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace kpz_lr3
{
    public partial class Admin : Form
    {
        private readonly string connectionString;
        private readonly bool readOnly;
        // Білий список: назви таблиць/стовпців не вводяться користувачем.
        private readonly string[] tables = { "Users", "Goals", "Intensities", "Workouts", "Exercises", "WorkoutExercises", "Requests" };
        private readonly string[][] columns = {
            new[] { "Id", "FullName", "Age", "HeightCm", "WeightKg", "ExperienceLevel" },
            new[] { "Id", "Name" },
            new[] { "Id", "Name" },
            new[] { "Id", "Name", "GoalId", "IntensityId", "SessionsPerWeek", "RequiredLevel", "Description" },
            new[] { "Id", "Name", "Equipment", "Instructions" },
            new[] { "WorkoutId", "ExerciseId", "ExerciseOrder", "Sets", "Repetitions", "DurationSeconds", "RestSeconds" },
            new[] { "Id", "UserId", "GoalId", "IntensityId", "SessionsPerWeek", "ExperienceAtRequest", "CreatedAt" }
        };
        private readonly Dictionary<string, Control> inputs = new Dictionary<string, Control>();
        private DataRow selectedRow;
        private bool reportMode;
        private bool loading;
        private int TableIndex { get { return cmbTable.SelectedIndex; } }
        private string TableName { get { return tables[TableIndex]; } }
        private string[] ColumnNames { get { return columns[TableIndex]; } }
        private string[] Keys { get { return TableName == "WorkoutExercises" ? new[] { "WorkoutId", "ExerciseId" } : new[] { "Id" }; } }
        private bool IsIdentity(string name) { return name == "Id" && (TableName == "Users" || TableName == "Requests"); }

        // Цей конструктор потрібен також дизайнеру Visual Studio.
        public Admin() : this("Server=localhost;Database=My db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;Connect Timeout=10;") { }
        public Admin(string connection, bool readOnly = false)
        {
            InitializeComponent();
            connectionString = connection;
            this.readOnly = readOnly;
            if (readOnly)
            {
                btnNew.Visible = btnInsert.Visible = btnUpdate.Visible = btnDelete.Visible = false;
                this.Text += " — тільки перегляд";
            }
            cmbTable.Items.AddRange(tables);
            cmbReport.Items.AddRange(new object[] {
                "JOIN: тренування, мета та інтенсивність",
                "GROUP BY / HAVING: цілі з тренуваннями",
                "Підзапит AVG: користувачі важчі за середню вагу",
                "Підзапит IN: вправи для мети (ID = GoalId)",
                "Підзапит NOT EXISTS: користувачі без запитів",
                "Корельований COUNT: кількість вправ у плані",
                "Процедура: пошук планів за метою (0 = усі)",
                "Процедура: історія користувача (ID = UserId)",
                "Журнал: зміни тренувань, записані тригером"
            });
            cmbReport.SelectedIndex = 0;
            cmbTable.SelectedIndexChanged += TableChanged;
            btnSearch.Click += SearchClick;
            btnNew.Click += NewClick;
            btnInsert.Click += InsertClick;
            btnUpdate.Click += UpdateClick;
            btnDelete.Click += DeleteClick;
            btnReport.Click += ReportClick;
            btnBack.Click += BackClick;
            grid.SelectionChanged += GridSelectionChanged;
            Shown += AdminShown;
        }
        private void AdminShown(object sender, EventArgs e) { cmbTable.SelectedIndex = 0; }
        private void BackClick(object sender, EventArgs e) { Close(); }
        private void TableChanged(object sender, EventArgs e)
        {
            if (TableIndex < 0) return;
            txtSearch.Clear();
            loading = true;
            try { grid.DataSource = null; selectedRow = null; }
            finally { loading = false; }
            btnUpdate.Enabled = btnDelete.Enabled = false;
            LoadTableSafe();
        }
        private void SearchClick(object sender, EventArgs e) { LoadTableSafe(); }
        private void LoadTableSafe()
        {
            try { LoadTable(); }
            catch (Exception ex)
            {
                reportMode = true; selectedRow = null; fields.Enabled = false;
                btnNew.Enabled = btnInsert.Enabled = btnUpdate.Enabled = btnDelete.Enabled = false;
                ShowError(ex);
            }
        }

        // READ + WHERE + ORDER BY. Пошук буквального тексту, без шаблонів LIKE.
        private void LoadTable()
        {
            if (TableIndex < 0) return;
            // Оригінальні ключі залишаються в DataTable для UPDATE/DELETE.
            // Видимі назви додаються окремими стовпцями через LEFT JOIN.
            var select = ColumnNames.Select(c => "t.[" + c + "]").ToList();
            var joins = new List<string>();
            var searchable = ColumnNames.ToList();
            foreach (string name in ColumnNames)
            {
                string source = LookupSource(name);
                if (source != "")
                {
                    string alias = "ref_" + name;
                    string title = source == "Users" ? "FullName" : "Name";
                    joins.Add("LEFT JOIN dbo.[" + source + "] " + alias + " ON " + alias + ".Id=t.[" + name + "]");
                    select.Add(alias + ".[" + title + "] AS [" + name + "Text]");
                    searchable.Add(name + "Text");
                }
                else if (IsLevel(name))
                {
                    select.Add("CASE t.[" + name + "] WHEN 1 THEN N'Початківець' WHEN 2 THEN N'Середній' WHEN 3 THEN N'Досвідчений' END AS [" + name + "Text]");
                    searchable.Add(name + "Text");
                }
            }
            string condition = string.Join(" OR ", searchable.Select(c => "CHARINDEX(@text, CONVERT(nvarchar(max), [" + c + "])) > 0"));
            string sql = "SELECT * FROM (SELECT " + string.Join(", ", select) +
                " FROM dbo.[" + TableName + "] t " + string.Join(" ", joins) + ") q WHERE @text=N'' OR " + condition +
                " ORDER BY " + string.Join(", ", Keys.Select(c => "[" + c + "]"));
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.Add("@text", SqlDbType.NVarChar, 4000).Value = txtSearch.Text.Trim();
                DataTable data = Read(command);
                BuildEditors();
                ShowSql(command);
                reportMode = false;
                fields.Enabled = true;
                btnNew.Enabled = btnInsert.Enabled = true;
                BindGrid(data);
               
            }
        }
        private DataTable Read(SqlCommand command)
        {
            ShowSql(command);
            using (var connection = new SqlConnection(connectionString))
            {
                command.Connection = connection;
                using (var adapter = new SqlDataAdapter(command))
                {
                    var data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        private void BindGrid(DataTable data)
        {
            loading = true;
            try
            {
                grid.DataSource = data;
                selectedRow = null;
                if (!reportMode)
                {
                    int position = 0;
                    foreach (string name in ColumnNames)
                    {
                        string visible = IsChoice(name) ? name + "Text" : name;
                        grid.Columns[name].Visible = !IsChoice(name);
                        grid.Columns[visible].HeaderText = Caption(name);
                        grid.Columns[visible].DisplayIndex = position++;
                        grid.Columns[visible].SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
            }
            finally { loading = false; }
            if (!reportMode) SelectRow();
        }
        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (!loading && !reportMode) SelectRow();
        }
        private void SelectRow()
        {
            var view = grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as DataRowView;
            selectedRow = view == null ? null : view.Row;
            foreach (string name in ColumnNames)
            {
                bool locked = IsIdentity(name) || (selectedRow != null && Keys.Contains(name)) ||
                    (TableName == "Requests" && name == "UserId" && selectedRow != null);
                if (inputs[name] is ComboBox combo)
                {
                    if (selectedRow == null || selectedRow.IsNull(name)) combo.SelectedIndex = -1;
                    else combo.SelectedValue = selectedRow[name];
                    combo.Enabled = !locked;
                }
                else if (inputs[name] is TextBox input)
                {
                    input.Text = selectedRow == null || selectedRow.IsNull(name) ? "" :
                        (selectedRow[name] is DateTime date ? date.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : Convert.ToString(selectedRow[name], CultureInfo.CurrentCulture));
                    input.ReadOnly = locked;
                }
            }
            btnUpdate.Enabled = btnDelete.Enabled = selectedRow != null;
        }
        private void NewClick(object sender, EventArgs e)
        {
            loading = true;
            try { grid.ClearSelection(); grid.CurrentCell = null; selectedRow = null; }
            finally { loading = false; }
            foreach (string name in ColumnNames)
            {
                if (inputs[name] is ComboBox combo)
                {
                    combo.Enabled = true;
                    combo.SelectedIndex = -1;
                }
                else if (inputs[name] is TextBox input)
                {
                    input.Clear(); input.ReadOnly = IsIdentity(name);
                }
            }
            if (TableName == "Requests") inputs["CreatedAt"].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            btnUpdate.Enabled = btnDelete.Enabled = false;
            
        }

        private static bool IsLevel(string name)
        {
            return name == "ExperienceLevel" || name == "RequiredLevel" || name == "ExperienceAtRequest";
        }
        private static string LookupSource(string name)
        {
            switch (name)
            {
                case "GoalId": return "Goals";
                case "IntensityId": return "Intensities";
                case "UserId": return "Users";
                case "WorkoutId": return "Workouts";
                case "ExerciseId": return "Exercises";
                default: return "";
            }
        }
        private static bool IsChoice(string name) { return LookupSource(name) != "" || IsLevel(name); }
        private static string Caption(string name)
        {
            switch (name)
            {
                case "Id": return "ID";
                case "FullName": return "Користувач";
                case "Name": return "Назва";
                case "GoalId": return "Мета";
                case "IntensityId": return "Інтенсивність";
                case "UserId": return "Користувач";
                case "WorkoutId": return "Тренування";
                case "ExerciseId": return "Вправа";
                case "ExperienceLevel": return "Підготовка";
                case "RequiredLevel": return "Потрібний рівень";
                case "ExperienceAtRequest": return "Рівень на час запиту";
                case "Age": return "Вік";
                case "HeightCm": return "Зріст, см";
                case "WeightKg": return "Вага, кг";
                case "SessionsPerWeek": return "Занять/тиждень";
                case "Description": return "Опис";
                case "Equipment": return "Обладнання";
                case "Instructions": return "Виконання";
                case "ExerciseOrder": return "Порядок вправи";
                case "Sets": return "Підходи";
                case "Repetitions": return "Повторення";
                case "DurationSeconds": return "Тривалість, с";
                case "RestSeconds": return "Відпочинок, с";
                case "CreatedAt": return "Дата запиту";
                default: return name;
            }
        }
        private void BuildEditors()
        {
            // Спочатку читаємо довідники. Якщо SQL завершиться помилкою, старі поля не використовуємо.
            var choices = new Dictionary<string, DataTable>();
            foreach (string name in ColumnNames)
            {
                if (!IsChoice(name)) continue;
                DataTable data;
                if (IsLevel(name))
                {
                    data = new DataTable();
                    data.Columns.Add("Id", typeof(int)); data.Columns.Add("Title", typeof(string));
                    data.Rows.Add(1, "Початківець"); data.Rows.Add(2, "Середній"); data.Rows.Add(3, "Досвідчений");
                }
                else
                {
                    string source = LookupSource(name);
                    string title = source == "Users" ? "FullName" : "Name";
                    using (var command = new SqlCommand("SELECT Id, [" + title + "] AS Title FROM dbo.[" + source + "] ORDER BY [" + title + "], Id"))
                        data = Read(command);
                    // Однакові імена дозволені для Users/Workouts: розрізняємо тільки дублікати.
                    var duplicates = data.AsEnumerable().GroupBy(r => Convert.ToString(r["Title"]))
                        .Where(g => g.Count() > 1).Select(g => g.Key).ToHashSet();
                    foreach (DataRow row in data.Rows)
                        if (duplicates.Contains(Convert.ToString(row["Title"])))
                            row["Title"] = row["Title"] + " (ID " + row["Id"] + ")";
                }
                choices.Add(name, data);
            }
            fields.SuspendLayout();
            try
            {
                foreach (Control control in fields.Controls.Cast<Control>().ToArray()) control.Dispose();
                fields.Controls.Clear(); fields.RowStyles.Clear(); fields.RowCount = 0; inputs.Clear();
                foreach (string name in ColumnNames)
                {
                    int row = fields.RowCount++;
                    fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
                    var label = new Label { Text = Caption(name), AutoSize = true, Dock = DockStyle.Fill };
                    Control input;
                    if (choices.TryGetValue(name, out DataTable values))
                    {
                        var combo = new ComboBox
                        {
                            Dock = DockStyle.Top,
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            DisplayMember = "Title",
                            ValueMember = "Id",
                            DataSource = values,
                            DropDownWidth = 450
                        };
                        combo.SelectedIndex = -1;
                        input = combo;
                    }
                    else input = new TextBox { Dock = DockStyle.Top };
                    fields.Controls.Add(label, 0, row); fields.Controls.Add(input, 1, row);
                    inputs.Add(name, input);
                }
            }
            finally { fields.ResumeLayout(); }
        }

        // Параметри мають явні типи. Текст користувача не вставляється у SQL.
        private SqlParameter Parameter(string name, string column, object value)
        {
            SqlParameter p;
            if (column == "HeightCm" || column == "WeightKg")
            {
                p = new SqlParameter(name, SqlDbType.Decimal);
                p.Precision = (byte)(column == "HeightCm" ? 5 : 6); p.Scale = 2;
            }
            else if (column == "CreatedAt") { p = new SqlParameter(name, SqlDbType.DateTime2); p.Scale = 7; }
            else if (column == "FullName" || column == "Name" || column == "Equipment" || column == "Instructions" || column == "Description")
            {
                int size = 500;
                if (column == "FullName" || column == "Equipment") size = 100;
                if (column == "Name") size = TableName == "Goals" ? 80 : TableName == "Intensities" ? 40 : TableName == "Exercises" ? 120 : 150;
                p = new SqlParameter(name, SqlDbType.NVarChar, size);
            }
            else p = new SqlParameter(name, SqlDbType.Int);
            p.Value = value ?? DBNull.Value;
            return p;
        }
        private object InputValue(string name)
        {
            if (inputs[name] is ComboBox combo)
            {
                if (combo.SelectedIndex < 0 || combo.SelectedValue == null)
                    throw new Exception("Виберіть значення: " + Caption(name));
                return Convert.ToInt32(combo.SelectedValue);
            }
            string text = inputs[name].Text.Trim();
            SqlDbType type = Parameter("@test", name, null).SqlDbType;
            if (type == SqlDbType.NVarChar)
            {
                if (text.Length > Parameter("@test", name, null).Size) throw new Exception("Задовге значення: " + name);
                return text;
            }
            if (text == "" && (name == "Repetitions" || name == "DurationSeconds")) return DBNull.Value;
            if (type == SqlDbType.Decimal)
            {
                decimal number;
                if (!decimal.TryParse(text.Replace(',', '.'), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out number))
                    throw new Exception("Поле " + name + ": введіть число, наприклад 75,50.");
                return number;
            }
            if (type == SqlDbType.DateTime2)
            {
                DateTime date;
                if (!DateTime.TryParseExact(text, new[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.fffffff" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    throw new Exception("CreatedAt: формат yyyy-MM-dd HH:mm:ss.");
                return date;
            }
            int integer;
            if (!int.TryParse(text, out integer)) throw new Exception("Поле " + name + ": введіть ціле число.");
            return integer;
        }
        private void InsertClick(object sender, EventArgs e)
        {
            if (reportMode||readOnly) return;
            try
            {
                // CREATE: identity-стовпці виключено зі списку INSERT.
                string[] writable = ColumnNames.Where(c => !IsIdentity(c)).ToArray();
                using (var command = new SqlCommand())
                {
                    command.CommandText = "INSERT INTO dbo.[" + TableName + "] (" + string.Join(", ", writable.Select(c => "[" + c + "]")) +
                        ") VALUES (" + string.Join(", ", writable.Select(c => "@" + c)) + ");";
                    foreach (string name in writable) command.Parameters.Add(Parameter("@" + name, name, InputValue(name)));
                    Write(command, false);
                }
                AfterWrite("Запис додано.");
            }
            catch (Exception ex) { ShowError(ex); }
        }
        private void UpdateClick(object sender, EventArgs e)
        {
            if (readOnly ||reportMode || selectedRow == null) return;
            try
            {
                using (var command = new SqlCommand())
                {
                    // UPDATE: первинні ключі не змінюємо.
                    string[] writable = ColumnNames.Where(c => !Keys.Contains(c)).ToArray();
                    command.CommandText = "UPDATE dbo.[" + TableName + "] SET " + string.Join(", ", writable.Select(c => "[" + c + "] = @" + c));
                    foreach (string name in writable) command.Parameters.Add(Parameter("@" + name, name, InputValue(name)));
                    AddOriginalWhere(command);
                    Write(command, true);
                }
                AfterWrite("Запис змінено.");
            }
            catch (Exception ex) { ShowError(ex); }
        }
        private void DeleteClick(object sender, EventArgs e)
        {
            if (readOnly || reportMode || selectedRow == null) return;
            if (MessageBox.Show(this, "Видалити вибраний запис із " + TableName + "?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                using (var command = new SqlCommand("DELETE FROM dbo.[" + TableName + "]"))
                {
                    AddOriginalWhere(command);
                    Write(command, true);
                }
                AfterWrite("Запис видалено.");
            }
            catch (Exception ex) { ShowError(ex); }
        }
        // Ключі + старі значення: чужі зміни не перезаписуються мовчки.
        private void AddOriginalWhere(SqlCommand command)
        {
            var parts = new List<string>();
            foreach (string name in ColumnNames)
            {
                parts.Add("([" + name + "] = @old_" + name + " OR ([" + name + "] IS NULL AND @old_" + name + " IS NULL))");
                command.Parameters.Add(Parameter("@old_" + name, name, selectedRow[name]));
            }
            command.CommandText += " WHERE " + string.Join(" AND ", parts) + ";";
        }
        private void Write(SqlCommand command, bool checkRow)
        {
            command.CommandText += " SELECT @@ROWCOUNT;";
            ShowSql(command);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    command.Connection = connection; command.Transaction = transaction;
                    try
                    {
                        int affected = Convert.ToInt32(command.ExecuteScalar());
                        if (checkRow && affected != 1) throw new Exception("Запис уже змінено або видалено. Натисніть «Знайти / оновити».");
                        transaction.Commit();
                    }
                    catch { try { transaction.Rollback(); } catch { } throw; }
                }
            }
        }
        private void AfterWrite(string message)
        {
            string lastSql = txtSql.Text;
            // Збереження вже завершилось. Помилка оновлення списку — окрема подія.
            try { LoadTable(); }
            catch (Exception ex) { MessageBox.Show(this, message + " Але список не оновлено: " + ex.Message); }
            txtSql.Text = lastSql;
           
        }
        private void ShowSql(SqlCommand command)
        {
            string text = command.CommandType == CommandType.StoredProcedure ? "EXEC " + command.CommandText : command.CommandText;
            foreach (SqlParameter p in command.Parameters)
                text += Environment.NewLine + "-- " + p.ParameterName + " = " + (p.Value == DBNull.Value ? "NULL" : Convert.ToString(p.Value, CultureInfo.CurrentCulture));
            txtSql.Text = text;
        }
        private void ShowError(Exception ex)
        {
            string message = ex.Message;
            if (ex is SqlException sql && sql.Number == 547)
                message = "Порушено зв’язок або CHECK-обмеження. Перевірте ID, діапазони чисел. Перед видаленням батьківського запису видаліть залежні записи.\n\n" + message;
            MessageBox.Show(this, message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Навчальні запити. ID вводиться у numId, пошуковий текст — у txtSearch.
        private void ReportClick(object sender, EventArgs e)
        {
            try
            {
                using (var command = new SqlCommand())
                {
                    switch (cmbReport.SelectedIndex)
                    {
                        case 0:
                            command.CommandText = @"SELECT w.Id, w.Name, g.Name AS Goal, i.Name AS Intensity, w.SessionsPerWeek
FROM dbo.Workouts w JOIN dbo.Goals g ON g.Id=w.GoalId
JOIN dbo.Intensities i ON i.Id=w.IntensityId ORDER BY w.Name, w.Id;"; break;
                        case 1:
                            command.CommandText = @"SELECT g.Id, g.Name, COUNT(w.Id) AS WorkoutCount
FROM dbo.Goals g LEFT JOIN dbo.Workouts w ON w.GoalId=g.Id
GROUP BY g.Id, g.Name HAVING COUNT(w.Id)>0 ORDER BY WorkoutCount DESC, g.Id;"; break;
                        case 2:
                            command.CommandText = @"SELECT Id, FullName, WeightKg FROM dbo.Users
WHERE WeightKg > (SELECT AVG(WeightKg) FROM dbo.Users) ORDER BY WeightKg DESC, Id;"; break;
                        case 3:
                            command.CommandText = @"SELECT e.Id, e.Name FROM dbo.Exercises e
WHERE e.Id IN (SELECT we.ExerciseId FROM dbo.WorkoutExercises we
WHERE we.WorkoutId IN (SELECT w.Id FROM dbo.Workouts w WHERE w.GoalId=@id)) ORDER BY e.Name, e.Id;";
                            command.Parameters.Add("@id", SqlDbType.Int).Value = (int)numId.Value; break;
                        case 4:
                            command.CommandText = @"SELECT u.Id, u.FullName FROM dbo.Users u
WHERE NOT EXISTS (SELECT 1 FROM dbo.Requests r WHERE r.UserId=u.Id) ORDER BY u.FullName, u.Id;"; break;
                        case 5:
                            command.CommandText = @"SELECT w.Id, w.Name,
(SELECT COUNT(*) FROM dbo.WorkoutExercises we WHERE we.WorkoutId=w.Id) AS ExerciseCount
FROM dbo.Workouts w ORDER BY ExerciseCount DESC, w.Id;"; break;
                        case 6:
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "dbo.Admin_SearchWorkouts";
                            command.Parameters.Add("@GoalId", SqlDbType.Int).Value = (int)numId.Value;
                            command.Parameters.Add("@Text", SqlDbType.NVarChar, 4000).Value = txtSearch.Text.Trim(); break;
                        case 7:
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "dbo.Admin_GetUserRequests";
                            command.Parameters.Add("@UserId", SqlDbType.Int).Value = (int)numId.Value; break;
                        default:
                            command.CommandText = "SELECT TOP (200) * FROM dbo.AdminWorkoutAudit ORDER BY AuditId DESC;"; break;
                    }
                    DataTable data = Read(command);
                    reportMode = true;
                    selectedRow = null;
                    fields.Enabled = false;
                    btnNew.Enabled = btnInsert.Enabled = btnUpdate.Enabled = btnDelete.Enabled = false;
                    BindGrid(data);
                   
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }
    }
}
