using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;


namespace kpz_lr3
{
    public partial class Form1 : Form
    {

        private readonly string ConnectionString;
        private readonly bool isAdmin;
        //private readonly string ConnectionString = "Server=localhost;Database=My db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;Connect Timeout=10;";
        private DataTable catalog = new DataTable();
        private DataTable allExercises = new DataTable();
        private DataTable recommendations = new DataTable();
        private DataTable recommendationExercises = new DataTable();
        private bool loading;
        private int? userId;
        private string reportProfile = "";

        public Form1()
        {
            InitializeComponent();

            using (var login = new Login())
            {
                login.ShowDialog();
                isAdmin = login.logged_in;
            }

            SqlConnection conn;
            if (isAdmin)
                conn = GetDatabaseConnection("AdminConnection");
            else
                conn = GetDatabaseConnection("GuestConnection");
            ConnectionString = conn.ConnectionString;
            this.Text += isAdmin ? " — адміністратор" : " — гість (тільки перегляд)";
            var btnAdmin = new Button();
            btnAdmin.Location = new System.Drawing.Point(profile.ClientSize.Width - btnAdmin.Width - 15, 20);
            btnAdmin.Text = "Адмінка";
            btnAdmin.Size = new System.Drawing.Size(130, 32);
            profile.Controls.Add(btnAdmin);
            btnAdmin.Click += OpenAdminClick;
            cmbLevel.Items.AddRange(new object[] { "Початківець", "Середній", "Досвідчений" });
            cmbLevel.SelectedIndex = 0;
            cmbSort.Items.AddRange(new object[] { "Назва А–Я", "Назва Я–А", "Занять: менше → більше", "Занять: більше → менше", "Інтенсивність: нижча → вища" });
            cmbSort.SelectedIndex = 0;
            Shown += FormShown;
            btnRefresh.Click += RefreshClick;
            btnNew.Click += NewClick;
            cmbUser.SelectedIndexChanged += UserChanged;
            btnSave.Click += SaveClick;
            btnRecommend.Click += RecommendClick;
            txtSearch.TextChanged += FilterChanged;
            cmbFilterGoal.SelectedIndexChanged += FilterChanged;
            cmbFilterIntensity.SelectedIndexChanged += FilterChanged;
            numMaxSessions.ValueChanged += FilterChanged;
            cmbSort.SelectedIndexChanged += FilterChanged;
            btnReset.Click += ResetClick;
            gridCatalog.SelectionChanged += CatalogChanged;
            gridRecommendations.SelectionChanged += RecommendationChanged;
            btnWord.Click += WordClick;
            btnExcel.Click += ExcelClick;
            var btnReports = new Button();
            btnReports.Text = "Звіти";
            btnReports.Size = new System.Drawing.Size(130, 32);

            btnReports.Location = new System.Drawing.Point( profile.ClientSize.Width - 290, 20);


            profile.Controls.Add(btnReports);
            btnReports.BringToFront();
            btnReports.Click += OpenReportsClick;
        }


        private void OpenAdminClick(object sender, EventArgs e)
        {
            try
            {
                using (var admin = new Admin(ConnectionString))
                {
                    Hide();
                    try { admin.ShowDialog(); }
                    finally { Show(); Activate(); }
                }

                recommendations.Clear();
                recommendationExercises.Clear();
                reportProfile = "";
                LoadAll();
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Помилка"); }
        }

        private static SqlConnection GetDatabaseConnection(string name)
        {
            var settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            if (settings == null)
                throw new InvalidOperationException($"У App.config немає рядка з'єднання \"{name}\".");
     
            return new SqlConnection(settings.ConnectionString);
        }

        private void ShowError(Exception ex)
        {
            
            if (ex is SqlException sql && sql.Number == 229)
                MessageBox.Show(this, "Недостатньо прав. Увійдіть як адміністратор, щоб змінювати дані.",
                    "Доступ заборонено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(this, ex.Message, "Помилка");
        }
        private void OpenReportsClick(object sender, EventArgs e)
        {
            using (var reports = new kpz_lr3.Reports.Reports(ConnectionString))
            {
                reports.ShowDialog(this);
            }
        }
        private void FormShown(object sender, EventArgs e)
        {
            try
            {
                LoadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void RefreshClick(object sender, EventArgs e)
        {
            try
            {
                LoadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void NewClick(object sender, EventArgs e)
        {
            try
            {
                NewUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void UserChanged(object sender, EventArgs e)
        {
            try
            {
                SelectUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void SaveClick(object sender, EventArgs e)
        {
            try
            {
                SaveUser();
                LoadUsers(userId);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void RecommendClick(object sender, EventArgs e)
        {
            try
            {
                Recommend();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            try
            {
                FilterCatalog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void ResetClick(object sender, EventArgs e)
        {
            try
            {
                ResetFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void CatalogChanged(object sender, EventArgs e)
        {
            try
            {
                ShowExercises(gridCatalog, gridCatalogExercises, allExercises);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void RecommendationChanged(object sender, EventArgs e)
        {
            try
            {
                ShowExercises(gridRecommendations, gridExercises, recommendationExercises);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void WordClick(object sender, EventArgs e)
        {
            try
            {
                Export(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private void ExcelClick(object sender, EventArgs e)
        {
            try
            {
                Export(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка");
            }
        }

        private DataTable Query(string sql)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var adapter = new SqlDataAdapter(sql, connection);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private static void BindLookup(ComboBox combo, DataTable table, bool addAll = false)
        {
            var copy = table.Copy();
            if (addAll) { var row = copy.NewRow(); row["Id"] = 0; row["Name"] = "Усі"; copy.Rows.InsertAt(row, 0); }
            combo.DisplayMember = "Name";
            combo.ValueMember = "Id";
            combo.DataSource = copy;
        }

        private void LoadAll()
        {
            loading = true;
            try
            {
                var goals = Query("select Id, Name from dbo.Goals order by Id");
                var intensities = Query("select Id, Name from dbo.Intensities order by Id");
                BindLookup(cmbGoal, goals);
                BindLookup(cmbIntensity, intensities);
                BindLookup(cmbFilterGoal, goals, true);
                BindLookup(cmbFilterIntensity, intensities, true);
                catalog = Query(@"select w.Id as WorkoutId, w.Name, w.GoalId, g.Name as Goal,
                    w.IntensityId, i.Name as Intensity, w.SessionsPerWeek, w.RequiredLevel, w.Description
                    from dbo.Workouts w join dbo.Goals g on g.Id=w.GoalId
                    join dbo.Intensities i on i.Id=w.IntensityId");
                allExercises = Query(@"select we.WorkoutId, w.Name as WorkoutName, we.ExerciseOrder,
                    e.Name as ExerciseName, e.Equipment, we.Sets, we.Repetitions,
                    we.DurationSeconds, we.RestSeconds, e.Instructions
                    from dbo.WorkoutExercises we join dbo.Exercises e on e.Id=we.ExerciseId
                    join dbo.Workouts w on w.Id=we.WorkoutId");
                LoadUsers(userId);
            }
            finally { loading = false; }
            SelectUser();
            FilterCatalog();

        }

        private void LoadUsers(int? selected)
        {
            bool previous = loading;
            loading = true;
            try
            {
                var users = Query("select Id, FullName, Age, HeightCm, WeightKg, ExperienceLevel from dbo.Users order by FullName, Id");
                cmbUser.DisplayMember = "FullName";
                cmbUser.ValueMember = "Id";
                cmbUser.DataSource = users;
                if (selected.HasValue) cmbUser.SelectedValue = selected.Value;
                else cmbUser.SelectedIndex = -1;
            }
            finally { loading = previous; }
        }

        private void NewUser()
        {
            cmbUser.SelectedIndex = -1;
            userId = null;
            txtName.Clear(); numAge.Value = 22; numHeight.Value = 175; numWeight.Value = 70;
            cmbLevel.SelectedIndex = 0;

            txtName.Focus();
        }

        private void SelectUser()
        {
            if (loading) return;
            DataRowView row = cmbUser.SelectedItem as DataRowView;
            if (row == null) { userId = null; return; }
            userId = Convert.ToInt32(row["Id"]);
            txtName.Text = Convert.ToString(row["FullName"]);
            numAge.Value = Convert.ToDecimal(row["Age"]);
            numHeight.Value = Math.Clamp(Convert.ToDecimal(row["HeightCm"]), numHeight.Minimum, numHeight.Maximum);
            numWeight.Value = Math.Clamp(Convert.ToDecimal(row["WeightKg"]), numWeight.Minimum, numWeight.Maximum);
            cmbLevel.SelectedIndex = Convert.ToInt32(row["ExperienceLevel"]) - 1;
        }

        private void ValidateProfile()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) throw new InvalidOperationException("Введіть ім’я користувача.");
            if (cmbLevel.SelectedIndex < 0) throw new InvalidOperationException("Виберіть рівень підготовки.");
        }

        private int WriteUser(SqlConnection connection, SqlTransaction transaction)
        {
            string sql = userId.HasValue
                ? @"update dbo.Users set FullName=@name, Age=@age, HeightCm=@height, WeightKg=@weight,
                    ExperienceLevel=@level where Id=@id; if @@rowcount=0 throw 50010, N'Користувача вже видалено. Оновіть список.', 1; select @id;"
                : @"insert into dbo.Users(FullName,Age,HeightCm,WeightKg,ExperienceLevel)
                    values(@name,@age,@height,@weight,@level);
                    select convert(int, scope_identity());";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
            command.Parameters.Add("@age", SqlDbType.Int).Value = (int)numAge.Value;
            var height = command.Parameters.Add("@height", SqlDbType.Decimal);
            height.Precision = 5; height.Scale = 2; height.Value = numHeight.Value;
            var weight = command.Parameters.Add("@weight", SqlDbType.Decimal);
            weight.Precision = 6; weight.Scale = 2; weight.Value = numWeight.Value;
            command.Parameters.Add("@level", SqlDbType.Int).Value = cmbLevel.SelectedIndex + 1;
            if (userId.HasValue) command.Parameters.Add("@id", SqlDbType.Int).Value = userId.Value;
            return Convert.ToInt32(command.ExecuteScalar());
        }

        private void SaveUser()
        {
            ValidateProfile();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            int id = WriteUser(connection, transaction);
            transaction.Commit();
            userId = id;
        }

        private void Recommend()
        {
            ValidateProfile();
            if (cmbGoal.SelectedValue == null || cmbIntensity.SelectedValue == null)
                throw new InvalidOperationException("Оновіть дані та виберіть мету й інтенсивність.");
            int goal = Convert.ToInt32(cmbGoal.SelectedValue);
            int intensity = Convert.ToInt32(cmbIntensity.SelectedValue);
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            int id = WriteUser(connection, transaction);
            // Вибираємо три плани, зберігаємо запит та читаємо вправи.
            string sql = @"    declare @level int = (select ExperienceLevel from dbo.Users where Id = @user_id);

    -- Правило 1: мета обов'язково збігається.
    -- Правило 2: вимоги плану не перевищують підготовку користувача.
    -- Правило 3: кандидат повинен мати вправи.
    -- Правило 4: інтенсивність: 40 / 20 / 0 балів за різницю 0 / 1 / 2.
    -- Правило 5: частота: 30 за точний збіг, мінус 10 за кожен день різниці, мінімум 0.
    -- Правило 6: збіг рівня підготовки додає 10 балів.
    -- Максимум 80. Це рейтинг відповідності, а не відсоток ефективності.
    select top (3) w.Id as WorkoutId,
        40 - 20 * abs(w.IntensityId - @intensity_id)
        + case when abs(w.SessionsPerWeek - @sessions_per_week) >= 3 then 0
            else 30 - 10 * abs(w.SessionsPerWeek - @sessions_per_week) end
        + case when w.RequiredLevel = @level then 10 else 0 end as Score
    into #Best
    from dbo.Workouts w
    where w.GoalId = @goal_id and w.RequiredLevel <= @level
      and exists (select 1 from dbo.WorkoutExercises we where we.WorkoutId = w.Id)
    order by Score desc, w.Id asc;

    if (select count(*) from #Best) < 3
        throw 50005, N'Недостатньо відповідних планів: потрібно додати щонайменше 3 кандидати.', 1;

    insert into dbo.Requests (UserId, GoalId, IntensityId, SessionsPerWeek, ExperienceAtRequest)
    values (@user_id, @goal_id, @intensity_id, @sessions_per_week, @level);
    declare @request_id int = convert(int, scope_identity());

    -- Результат 1: рівно 3 плани з поясненням рейтингу.
    select row_number() over (order by b.Score desc, w.Id) as Place,
        @request_id as RequestId, w.Id as WorkoutId, w.Name,
        g.Name as Goal, i.Name as Intensity, w.SessionsPerWeek,
        w.RequiredLevel, b.Score,
        concat(N'Мета збігається; різниця інтенсивності: ', abs(w.IntensityId - @intensity_id),
            N'; різниця занять/тиждень: ', abs(w.SessionsPerWeek - @sessions_per_week),
            N'; потрібний рівень: ', w.RequiredLevel, N', ваш: ', @level) as Explanation,
        w.Description
    from #Best b
    join dbo.Workouts w on w.Id = b.WorkoutId
    join dbo.Goals g on g.Id = w.GoalId
    join dbo.Intensities i on i.Id = w.IntensityId
    order by b.Score desc, w.Id;

    -- Результат 2: вправи для кожного з цих трьох планів.
    select w.Id as WorkoutId, w.Name as WorkoutName,
        we.ExerciseOrder, e.Name as ExerciseName, e.Equipment,
        we.Sets, we.Repetitions, we.DurationSeconds, we.RestSeconds, e.Instructions
    from #Best b
    join dbo.Workouts w on w.Id = b.WorkoutId
    join dbo.WorkoutExercises we on we.WorkoutId = w.Id
    join dbo.Exercises e on e.Id = we.ExerciseId
    order by b.Score desc, w.Id, we.ExerciseOrder;";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.Add("@user_id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@goal_id", SqlDbType.Int).Value = goal;
            command.Parameters.Add("@intensity_id", SqlDbType.Int).Value = intensity;
            command.Parameters.Add("@sessions_per_week", SqlDbType.Int).Value = (int)numSessions.Value;
            using var adapter = new SqlDataAdapter(command);
            var data = new DataSet();
            adapter.Fill(data);
            if (data.Tables.Count != 2 || data.Tables[0].Rows.Count != 3)
                throw new InvalidOperationException("Не вдалося отримати три плани та їхні вправи.");
            transaction.Commit(); // зберегти транзакцію
            userId = id;
            recommendations = data.Tables[0];
            recommendationExercises = data.Tables[1];
            reportProfile = $"Користувач: {txtName.Text.Trim()} | Вік: {numAge.Value} | Зріст: {numHeight.Value} см | Вага: {numWeight.Value} кг\n" +
                $"Підготовка: {cmbLevel.Text} | Мета: {cmbGoal.Text} | Інтенсивність: {cmbIntensity.Text} | Занять/тиждень: {numSessions.Value}\n" +
                $"Підбір виконано: {DateTime.Now:dd.MM.yyyy HH:mm}";
            BindGrid(gridRecommendations, recommendations.DefaultView);
            ShowExercises(gridRecommendations, gridExercises, recommendationExercises);
            LoadUsers(id);
            tabs.SelectedTab = tabRecommendations;

        }

        private bool ContainsText(object value, string search)
        {
            string text = Convert.ToString(value);
            return text.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void FilterCatalog()
        {
            if (loading || !catalog.Columns.Contains("Name")) return;
            string search = txtSearch.Text.Trim();
            int goal = 0;
            if (cmbFilterGoal.SelectedValue != null) goal = Convert.ToInt32(cmbFilterGoal.SelectedValue);
            int intensity = 0;
            if (cmbFilterIntensity.SelectedValue != null) intensity = Convert.ToInt32(cmbFilterIntensity.SelectedValue);
            var filtered = catalog.Clone();
            foreach (DataRow row in catalog.Rows)
            {
                int workoutId = Convert.ToInt32(row["WorkoutId"]);
                bool matchesText = ContainsText(row["Name"], search)
                    || ContainsText(row["Goal"], search)
                    || ContainsText(row["Description"], search);
                foreach (DataRow exercise in allExercises.Rows)
                {
                    if (Convert.ToInt32(exercise["WorkoutId"]) == workoutId
                        && (ContainsText(exercise["ExerciseName"], search)
                        || ContainsText(exercise["Equipment"], search)))
                    {
                        matchesText = true;
                        break;
                    }
                }
                if (matchesText && (goal == 0 || Convert.ToInt32(row["GoalId"]) == goal) &&
                    (intensity == 0 || Convert.ToInt32(row["IntensityId"]) == intensity) &&
                    Convert.ToInt32(row["SessionsPerWeek"]) <= numMaxSessions.Value)
                    filtered.ImportRow(row);
            }
            string sort = "Name asc, WorkoutId asc";
            if (cmbSort.SelectedIndex == 1) sort = "Name desc, WorkoutId asc";
            if (cmbSort.SelectedIndex == 2) sort = "SessionsPerWeek asc, Name asc";
            if (cmbSort.SelectedIndex == 3) sort = "SessionsPerWeek desc, Name asc";
            if (cmbSort.SelectedIndex == 4) sort = "IntensityId asc, Name asc";
            filtered.DefaultView.Sort = sort;
            BindGrid(gridCatalog, filtered.DefaultView);
            ShowExercises(gridCatalog, gridCatalogExercises, allExercises);
            lblCount.Text = $"Знайдено планів: {filtered.Rows.Count} із {catalog.Rows.Count}";
        }

        private void ResetFilters()
        {
            loading = true;
            txtSearch.Clear(); cmbFilterGoal.SelectedIndex = 0; cmbFilterIntensity.SelectedIndex = 0;
            numMaxSessions.Value = 7; cmbSort.SelectedIndex = 0;
            loading = false;
            FilterCatalog();
        }

        private static void ShowExercises(DataGridView parent, DataGridView target, DataTable source)
        {
            DataRowView row = parent.CurrentRow?.DataBoundItem as DataRowView;
            if (row == null || !source.Columns.Contains("WorkoutId"))
            { target.DataSource = null; return; }
            int id = Convert.ToInt32(row["WorkoutId"]);
            BindGrid(target, new DataView(source) { RowFilter = "WorkoutId = " + id, Sort = "ExerciseOrder asc" });
        }

        private static readonly Dictionary<string, string> Headers = new Dictionary<string, string>
        {
            ["Place"] = "Місце",
            ["WorkoutId"] = "ID плану",
            ["RequestId"] = "ID запиту",
            ["Name"] = "Назва плану",
            ["Goal"] = "Мета",
            ["Intensity"] = "Інтенсивність",
            ["SessionsPerWeek"] = "Занять/тиждень",
            ["RequiredLevel"] = "Потрібний рівень",
            ["Score"] = "Бали / 80",
            ["Explanation"] = "Пояснення",
            ["Description"] = "Опис",
            ["WorkoutName"] = "План",
            ["ExerciseOrder"] = "Порядок",
            ["ExerciseName"] = "Вправа",
            ["Equipment"] = "Обладнання",
            ["Sets"] = "Підходи",
            ["Repetitions"] = "Повторення",
            ["DurationSeconds"] = "Час/підхід, с",
            ["RestSeconds"] = "Відпочинок, с",
            ["Instructions"] = "Виконання"
        };
        private static void BindGrid(DataGridView grid, DataView view)
        {
            grid.DataSource = view;
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (Headers.TryGetValue(column.Name, out string title)) column.HeaderText = title;
                column.Visible = column.Name != "GoalId" && column.Name != "IntensityId" &&
                    column.Name != "RequestId" && column.Name != "WorkoutName";
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        private static DataTable ExportTable(DataTable source, params string[] columns)
        {
            var result = new DataTable();
            foreach (string column in columns)
                result.Columns.Add(Headers.TryGetValue(column, out string title) ? title : column, source.Columns[column].DataType);
            foreach (DataRow row in source.Rows)
            {
                DataRow newRow = result.NewRow();
                for (int i = 0; i < columns.Length; i++) newRow[i] = row[columns[i]];
                result.Rows.Add(newRow);
            }
            return result;
        }

        private void Export(bool excel)
        {
            XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            if (recommendations.Rows.Count != 3 || string.IsNullOrWhiteSpace(reportProfile))
                throw new InvalidOperationException("Спочатку натисніть кнопку підбору трьох тренувань.");

            using var dialog = new SaveFileDialog
            {
                Filter = excel ? "Excel (*.xlsx)|*.xlsx" : "Word (*.docx)|*.docx",
                DefaultExt = excel ? "xlsx" : "docx",
                AddExtension = true,
                FileName = "Підбір_тренувань_" + DateTime.Now.ToString("yyyyMMdd_HHmm"),
                OverwritePrompt = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            string temp = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(dialog.FileName)!, Guid.NewGuid() + ".tmp");
            try
            {
             
                if (excel)
                    Excel(temp, reportProfile, recommendations, recommendationExercises);
                else
                    Word(temp, reportProfile, recommendations, recommendationExercises);

                System.IO.File.Move(temp, dialog.FileName, true);
            }
            finally
            {
                if (System.IO.File.Exists(temp)) System.IO.File.Delete(temp);
            }
           
            MessageBox.Show(this, "Звіт збережено:\n" + dialog.FileName, "Експорт завершено");
            string Text(object? value) => new string(
                (Convert.ToString(value, CultureInfo.CurrentCulture) ?? "")
                .Where(System.Xml.XmlConvert.IsXmlChar).ToArray());

            string Value(DataRow row, string column) => Text(row[column]);

            DataRow[] Plans(DataTable plans)
            {
                if (plans.Rows.Count != 3)
                    throw new InvalidOperationException("Спочатку виконайте підбір трьох тренувань.");
                // Експортуємо в порядку рейтингу, незалежно від сортування таблиці на формі.
                return plans.AsEnumerable().OrderBy(r => Convert.ToInt32(r["Place"])).ToArray();
            }

            List<string[]> Profile(string context)
            {
                var result = new List<string[]>();
                foreach (string part in context.Split(new[] { '\n', '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int colon = part.IndexOf(':');
                    if (colon > 0)
                        result.Add(new[] { part.Substring(0, colon).Trim(), part.Substring(colon + 1).Trim() });
                }
                return result;
            }

            DataRow[] Exercises(DataTable exercises, DataRow plan) => exercises.AsEnumerable()
                .Where(r => Convert.ToInt32(r["WorkoutId"]) == Convert.ToInt32(plan["WorkoutId"]))
                .OrderBy(r => Convert.ToInt32(r["ExerciseOrder"])).ToArray();

            string Dose(DataRow exercise) => !exercise.IsNull("Repetitions")
                ? Value(exercise, "Repetitions") + " повт."
                : Value(exercise, "DurationSeconds") + " с";

            string Details(DataRow plan) =>
                "Мета: " + Value(plan, "Goal") + "; інтенсивність: " + Value(plan, "Intensity") +
                "; занять/тиждень: " + Value(plan, "SessionsPerWeek") +
                "; рівень: " + Value(plan, "RequiredLevel") + "; бали: " + Value(plan, "Score") + "/80";

            void Excel(string path, string profile, DataTable plans, DataTable exercises)
            {
                DataRow[] best = Plans(plans);
                using var book = new ClosedXML.Excel.XLWorkbook();
                var sheet = book.Worksheets.Add("Підбір тренувань");
                sheet.Style.Font.FontName = "Calibri";
                sheet.Style.Font.FontSize = 11;
                sheet.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                sheet.Style.Alignment.WrapText = true;
                double[] widths = { 5, 37, 25, 11, 16, 15 };
                for (int c = 0; c < widths.Length; c++) sheet.Column(c + 1).Width = widths[c];
                int row = 1;

                void Merged(string text, bool heading = false)
                {
                    var range = sheet.Range(row, 1, row, 6).Merge();
                    range.FirstCell().Value = text;
                    range.Style.Font.Bold = heading;
                    range.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml(heading ? "DCE6F1" : "FFFFFF");
                    // Merged cells do not reliably auto-fit. Reserve height for wrapped text.
                    sheet.Row(row).Height = Math.Max(24, 16 * (1 + text.Length / 110));
                    row++;
                }

                void Cells(string[] values, bool header = false)
                {
                    for (int c = 0; c < values.Length; c++) sheet.Cell(row, c + 1).Value = values[c];
                    var range = sheet.Range(row, 1, row, 6);
                    range.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.FromHtml("D9D9D9");
                    range.Style.Border.InsideBorderColor = ClosedXML.Excel.XLColor.FromHtml("D9D9D9");
                    range.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml(header ? "DCE6F1" : "FFFFFF");
                    range.Style.Font.Bold = header;
                    foreach (int c in new[] { 1, 4, 5, 6 })
                        sheet.Cell(row, c).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    int lines = 1;
                    for (int c = 0; c < values.Length; c++)
                        lines = Math.Max(lines, 1 + values[c].Length / Math.Max(1, (int)widths[c] - 2));
                    sheet.Row(row).Height = Math.Max(24, lines * 16);
                    row++;
                }

                Merged("Підбір тренувань", true);
                sheet.Cell(1, 1).Style.Font.FontSize = 16;
                foreach (string[] item in Profile(profile))
                {
                    var label = sheet.Range(row, 1, row, 2).Merge();
                    label.FirstCell().Value = item[0];
                    label.Style.Font.Bold = true;
                    label.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("F2F5F8");
                    sheet.Range(row, 3, row, 6).Merge().FirstCell().Value = item[1];
                    var range = sheet.Range(row, 1, row, 6);
                    range.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.FromHtml("D9D9D9");
                    sheet.Row(row).Height = Math.Max(21, 16 * (1 + item[1].Length / 60));
                    row++;
                }
                foreach (DataRow plan in best)
                {
                    sheet.Row(row++).Height = 7;
                    Merged("Варіант " + Value(plan, "Place") + " — " + Value(plan, "Name"), true);
                    Merged(Details(plan));
                    Cells(new[] { "№", "Вправа", "Обладнання", "Підходи", "Повт. / час", "Відпочинок, с" }, true);
                    foreach (DataRow exercise in Exercises(exercises, plan))
                    {
                        Cells(new[] { Value(exercise, "ExerciseOrder"), Value(exercise, "ExerciseName"),
                        Value(exercise, "Equipment"), Value(exercise, "Sets"), Dose(exercise), Value(exercise, "RestSeconds") });
                        // Числові клітинки залишаються числами, назви — текстом (не формулами).
                        sheet.Cell(row - 1, 1).Value = Convert.ToInt32(exercise["ExerciseOrder"]);
                        sheet.Cell(row - 1, 4).Value = Convert.ToInt32(exercise["Sets"]);
                        sheet.Cell(row - 1, 6).Value = Convert.ToInt32(exercise["RestSeconds"]);
                    }
                }
                sheet.ShowGridLines = false;
                sheet.SheetView.FreezeRows(1);
                sheet.PageSetup.PageOrientation = ClosedXML.Excel.XLPageOrientation.Portrait;
                sheet.PageSetup.PaperSize = ClosedXML.Excel.XLPaperSize.A4Paper;
                sheet.PageSetup.Margins.Top = 0.25;
                sheet.PageSetup.Margins.Bottom = 0.25;
                sheet.PageSetup.Margins.Left = 0.25;
                sheet.PageSetup.Margins.Right = 0.25;
                sheet.PageSetup.CenterHorizontally = true;
                sheet.PageSetup.PrintAreas.Add("A1:F" + (row - 1));
                sheet.PageSetup.FitToPages(1, 1);
                // Передаємо потік: тимчасовий файл має розширення .tmp, а вміст — XLSX.
                using var output = File.Create(path);
                book.SaveAs(output);
            }

            XElement Paragraph(string text, bool bold = false, int size = 20, bool keepNext = false)
            {
                return new XElement(W + "p",
                    new XElement(W + "pPr",
                        keepNext ? new XElement(W + "keepNext") : null,
                        new XElement(W + "spacing", new XAttribute(W + "after", 40),
                            new XAttribute(W + "line", 240), new XAttribute(W + "lineRule", "auto"))),
                    new XElement(W + "r", new XElement(W + "rPr",
                        new XElement(W + "rFonts", new XAttribute(W + "ascii", "Calibri"), new XAttribute(W + "hAnsi", "Calibri")),
                        bold ? new XElement(W + "b") : null,
                        new XElement(W + "color", new XAttribute(W + "val", "000000")),
                        new XElement(W + "sz", new XAttribute(W + "val", size))),
                        new XElement(W + "t", new XAttribute(XNamespace.Xml + "space", "preserve"), Text(text))));
            }

            XElement Table(List<string[]> rows, int[] widths, bool header)
            {
                var borders = new XElement(W + "tblBorders");
                foreach (string side in new[] { "top", "left", "bottom", "right", "insideH", "insideV" })
                    borders.Add(new XElement(W + side, new XAttribute(W + "val", "single"),
                        new XAttribute(W + "sz", 4), new XAttribute(W + "color", "D9D9D9")));
                var margins = new XElement(W + "tblCellMar");
                foreach (string side in new[] { "top", "left", "bottom", "right" })
                    margins.Add(new XElement(W + side, new XAttribute(W + "w", side == "left" || side == "right" ? 80 : 40),
                        new XAttribute(W + "type", "dxa")));
                var table = new XElement(W + "tbl", new XElement(W + "tblPr",
                    new XElement(W + "tblW", new XAttribute(W + "w", widths.Sum()), new XAttribute(W + "type", "dxa")),
                    borders, new XElement(W + "tblLayout", new XAttribute(W + "type", "fixed")), margins),
                    new XElement(W + "tblGrid", widths.Select(width => new XElement(W + "gridCol", new XAttribute(W + "w", width)))));
                for (int r = 0; r < rows.Count; r++)
                {
                    bool isHeader = header && r == 0;
                    var tr = new XElement(W + "tr", new XElement(W + "trPr", new XElement(W + "cantSplit"),
                        isHeader ? new XElement(W + "tblHeader") : null));
                    for (int c = 0; c < widths.Length; c++)
                    {
                        var paragraph = Paragraph(rows[r][c], isHeader || (!header && c % 2 == 0), 19, isHeader);
                        if (header && c != 1 && c != 2)
                            paragraph.Element(W + "pPr")!.Add(new XElement(W + "jc", new XAttribute(W + "val", "center")));
                        tr.Add(new XElement(W + "tc", new XElement(W + "tcPr",
                            new XElement(W + "tcW", new XAttribute(W + "w", widths[c]), new XAttribute(W + "type", "dxa")),
                            new XElement(W + "shd", new XAttribute(W + "val", "clear"), new XAttribute(W + "fill", isHeader ? "DCE6F1" : "FFFFFF")),
                            new XElement(W + "vAlign", new XAttribute(W + "val", "center"))), paragraph));
                    }
                    table.Add(tr);
                }
                return table;
            }

            void Word(string path, string profile, DataTable plans, DataTable exercises)
            {
                DataRow[] best = Plans(plans);
                var title = Paragraph("Підбір тренувань", true, 28, true);
                title.Element(W + "pPr")!.AddFirst(new XElement(W + "pStyle", new XAttribute(W + "val", "Title")));
                var body = new XElement(W + "body", title);
                List<string[]> pairs = Profile(profile);
                var profileRows = new List<string[]>();
                for (int i = 0; i < pairs.Count; i += 2)
                    profileRows.Add(new[] { pairs[i][0], pairs[i][1], i + 1 < pairs.Count ? pairs[i + 1][0] : "",
                    i + 1 < pairs.Count ? pairs[i + 1][1] : "" });
                body.Add(Table(profileRows, new[] { 1800, 3300, 1800, 3300 }, false));
                foreach (DataRow plan in best)
                {
                    var heading = Paragraph("Варіант " + Value(plan, "Place") + " — " + Value(plan, "Name"), true, 22, true);
                    heading.Element(W + "pPr")!.Element(W + "spacing")!.Add(new XAttribute(W + "before", 100));
                    body.Add(heading, Paragraph(Details(plan), false, 19, true));
                    var rows = new List<string[]> { new[] { "№", "Вправа", "Обладнання", "Підходи", "Повт. / час", "Відпочинок, с" } };
                    foreach (DataRow exercise in Exercises(exercises, plan))
                        rows.Add(new[] { Value(exercise, "ExerciseOrder"), Value(exercise, "ExerciseName"),
                        Value(exercise, "Equipment"), Value(exercise, "Sets"), Dose(exercise), Value(exercise, "RestSeconds") });
                    body.Add(Table(rows, new[] { 450, 3350, 2200, 1000, 1500, 1700 }, true));
                }
                // A4, поля 15 мм. Рядки таблиць переносяться цілком; текст не обрізається.
                body.Add(new XElement(W + "sectPr",
                    new XElement(W + "pgSz", new XAttribute(W + "w", 11906), new XAttribute(W + "h", 16838)),
                    new XElement(W + "pgMar", new XAttribute(W + "top", 850), new XAttribute(W + "bottom", 850),
                        new XAttribute(W + "left", 850), new XAttribute(W + "right", 850))));
                using var zip = ZipFile.Open(path, ZipArchiveMode.Create);
                XNamespace ct = "http://schemas.openxmlformats.org/package/2006/content-types";
                XNamespace rel = "http://schemas.openxmlformats.org/package/2006/relationships";
                Put(zip, "[Content_Types].xml", new XElement(ct + "Types",
                    new XElement(ct + "Default", new XAttribute("Extension", "rels"), new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
                    new XElement(ct + "Default", new XAttribute("Extension", "xml"), new XAttribute("ContentType", "application/xml")),
                    new XElement(ct + "Override", new XAttribute("PartName", "/word/document.xml"), new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml")),
                    new XElement(ct + "Override", new XAttribute("PartName", "/word/styles.xml"), new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"))));
                Put(zip, "_rels/.rels", new XElement(rel + "Relationships", new XElement(rel + "Relationship",
                    new XAttribute("Id", "rId1"), new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"), new XAttribute("Target", "word/document.xml"))));
                Put(zip, "word/_rels/document.xml.rels", new XElement(rel + "Relationships", new XElement(rel + "Relationship",
                    new XAttribute("Id", "rId1"), new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles"), new XAttribute("Target", "styles.xml"))));
                Put(zip, "word/styles.xml", new XElement(W + "styles", new XElement(W + "style", new XAttribute(W + "type", "paragraph"),
                    new XAttribute(W + "styleId", "Title"), new XElement(W + "name", new XAttribute(W + "val", "Title")))));
                Put(zip, "word/document.xml", new XElement(W + "document", new XAttribute(XNamespace.Xmlns + "w", W), body));
            }

            void Put(ZipArchive zip, string name, XElement root)
            {
                using var stream = zip.CreateEntry(name).Open();
                new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root).Save(stream);
            }
        }


        private void lblResult_Click(object sender, EventArgs e)
        {

        }

        private void lblHint_Click(object sender, EventArgs e)
        {

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {

        }

        private void btnAdmin_Click_1(object sender, EventArgs e)
        {

        }

       

    }

    // Справжні Office Open XML файли: ZIP + XML. Microsoft Office/Interop не потрібні.
    internal static class OfficeExport
    {
        private static readonly XNamespace Rel = "http://schemas.openxmlformats.org/package/2006/relationships";
        private static readonly XNamespace OfficeRel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        private static readonly XNamespace Ct = "http://schemas.openxmlformats.org/package/2006/content-types";
        private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        private static readonly XNamespace S = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private static string Clean(object value) => new string(Convert.ToString(value, CultureInfo.CurrentCulture)
            .Where(c => System.Xml.XmlConvert.IsXmlChar(c)).ToArray());
        private static void Put(ZipArchive zip, string path, XElement root)
        {
            using var stream = zip.CreateEntry(path).Open();
            new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root).Save(stream);
        }
        private static XElement Relationship(string id, string type, string target) => new XElement(Rel + "Relationship",
            new XAttribute("Id", id), new XAttribute("Type", OfficeRel.NamespaceName + "/" + type), new XAttribute("Target", target));
        private static XElement Types(params XElement[] parts) => new XElement(Ct + "Types",
            new XElement(Ct + "Default", new XAttribute("Extension", "rels"), new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
            new XElement(Ct + "Default", new XAttribute("Extension", "xml"), new XAttribute("ContentType", "application/xml")), parts);
        private static XElement Part(string name, string type) => new XElement(Ct + "Override",
            new XAttribute("PartName", name), new XAttribute("ContentType", type));
        private static XElement Paragraph(string text, bool bold = false) => new XElement(W + "p",
            new XElement(W + "r", bold ? new XElement(W + "rPr", new XElement(W + "b")) : null,
                new XElement(W + "t", new XAttribute(XNamespace.Xml + "space", "preserve"), Clean(text))));
        public static void Word(string path, string context, DataTable plans, DataTable exercises)
        {
            using var zip = ZipFile.Open(path, ZipArchiveMode.Create);
            Put(zip, "[Content_Types].xml", Types(Part("/word/document.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml")));
            Put(zip, "_rels/.rels", new XElement(Rel + "Relationships", Relationship("rId1", "officeDocument", "word/document.xml")));
            var body = new XElement(W + "body", Paragraph("Підбір тренувань", true));
            foreach (string line in context.Split('\n')) body.Add(Paragraph(line));
            body.Add(Paragraph("Навчальна модель. Бали показують відповідність правилам, а не гарантовану ефективність. Вік, зріст і вага зберігаються в анкеті та не впливають на рейтинг."));
            foreach (DataRow plan in plans.Rows)
            {
                body.Add(Paragraph(Convert.ToString(plan["Назва плану"]), true));
                foreach (DataColumn column in plans.Columns)
                    if (column.ColumnName != "Назва плану") body.Add(Paragraph(column.ColumnName + ": " + Clean(plan[column])));
                body.Add(Paragraph("Вправи", true));
                foreach (DataRow exercise in exercises.Rows)
                    if (Equals(exercise["ID плану"], plan["ID плану"]))
                    {
                        body.Add(Paragraph(exercise["Порядок"] + ". " + exercise["Вправа"], true));
                        foreach (DataColumn column in exercises.Columns)
                            if (column.ColumnName != "ID плану" && column.ColumnName != "Порядок" && column.ColumnName != "Вправа" && !exercise.IsNull(column))
                                body.Add(Paragraph(column.ColumnName + ": " + Clean(exercise[column])));
                    }
            }
            body.Add(new XElement(W + "sectPr", new XElement(W + "pgSz", new XAttribute(W + "w", 11906), new XAttribute(W + "h", 16838)),
                new XElement(W + "pgMar", new XAttribute(W + "top", 1134), new XAttribute(W + "bottom", 1134), new XAttribute(W + "left", 1134), new XAttribute(W + "right", 1134))));
            Put(zip, "word/document.xml", new XElement(W + "document", new XAttribute(XNamespace.Xmlns + "w", W), body));
        }
        public static void Excel(string path, string context, DataTable plans, DataTable exercises)
        {
            var info = new DataTable(); info.Columns.Add("Інформація");
            foreach (string line in context.Split('\n')) info.Rows.Add(line);
            info.Rows.Add("Навчальна модель. Максимум 80 балів. Антропометричні дані не впливають на рейтинг.");
            var tables = new[] { info, plans, exercises };
            string[] names = { "Анкета та параметри", "Плани", "Вправи" };
            using var zip = ZipFile.Open(path, ZipArchiveMode.Create);
            var types = Types(Part("/xl/workbook.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"));
            var sheets = new XElement(S + "sheets");
            var relationships = new XElement(Rel + "Relationships");
            for (int i = 0; i < tables.Length; i++)
            {
                string id = "rId" + (i + 1);
                sheets.Add(new XElement(S + "sheet", new XAttribute("name", names[i]), new XAttribute("sheetId", i + 1), new XAttribute(OfficeRel + "id", id)));
                relationships.Add(Relationship(id, "worksheet", $"worksheets/sheet{i + 1}.xml"));
                types.Add(Part($"/xl/worksheets/sheet{i + 1}.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"));
                Put(zip, $"xl/worksheets/sheet{i + 1}.xml", Sheet(tables[i]));
            }
            Put(zip, "[Content_Types].xml", types);
            Put(zip, "_rels/.rels", new XElement(Rel + "Relationships", Relationship("rId1", "officeDocument", "xl/workbook.xml")));
            Put(zip, "xl/workbook.xml", new XElement(S + "workbook", new XAttribute(XNamespace.Xmlns + "r", OfficeRel), sheets));
            Put(zip, "xl/_rels/workbook.xml.rels", relationships);
        }
        private static string Column(int number)
        {
            string result = "";
            while (number > 0) { number--; result = (char)('A' + number % 26) + result; number /= 26; }
            return result;
        }
        private static XElement Cell(string address, object value)
        {
            var cell = new XElement(S + "c", new XAttribute("r", address));
            if (value is DBNull) return cell;
            if (value is byte || value is short || value is int || value is long || value is decimal || value is double)
                cell.Add(new XElement(S + "v", Convert.ToString(value, CultureInfo.InvariantCulture)));
            else cell.Add(new XAttribute("t", "inlineStr"), new XElement(S + "is", new XElement(S + "t",
                new XAttribute(XNamespace.Xml + "space", "preserve"), Clean(value))));
            return cell; // Текст '=...' залишається текстом, а не формулою.
        }
        private static XElement Sheet(DataTable table)
        {
            var data = new XElement(S + "sheetData");
            var header = new XElement(S + "row", new XAttribute("r", 1));
            for (int c = 0; c < table.Columns.Count; c++) header.Add(Cell(Column(c + 1) + "1", table.Columns[c].ColumnName));
            data.Add(header);
            for (int r = 0; r < table.Rows.Count; r++)
            {
                var row = new XElement(S + "row", new XAttribute("r", r + 2));
                for (int c = 0; c < table.Columns.Count; c++) row.Add(Cell(Column(c + 1) + (r + 2), table.Rows[r][c]));
                data.Add(row);
            }
            return new XElement(S + "worksheet",
                new XElement(S + "sheetViews", new XElement(S + "sheetView", new XAttribute("workbookViewId", 0),
                    new XElement(S + "pane", new XAttribute("ySplit", 1), new XAttribute("topLeftCell", "A2"), new XAttribute("activePane", "bottomLeft"), new XAttribute("state", "frozen")))),
                new XElement(S + "cols", new XElement(S + "col", new XAttribute("min", 1), new XAttribute("max", table.Columns.Count), new XAttribute("width", 28), new XAttribute("customWidth", 1))),
                data, new XElement(S + "autoFilter", new XAttribute("ref", "A1:" + Column(table.Columns.Count) + (table.Rows.Count + 1))));
        }
    }
}
