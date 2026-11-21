using System;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Windows.Forms;

namespace kpz_lr3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            profile = new GroupBox();
            lblUser = new Label();
            cmbUser = new ComboBox();
            btnNew = new Button();
            btnRefresh = new Button();
            lblName = new Label();
            txtName = new TextBox();
            lblAge = new Label();
            numAge = new NumericUpDown();
            lblHeight = new Label();
            numHeight = new NumericUpDown();
            lblWeight = new Label();
            numWeight = new NumericUpDown();
            lblLevel = new Label();
            cmbLevel = new ComboBox();
            lblGoal = new Label();
            cmbGoal = new ComboBox();
            lblIntensity = new Label();
            cmbIntensity = new ComboBox();
            lblSessions = new Label();
            numSessions = new NumericUpDown();
            btnSave = new Button();
            btnRecommend = new Button();
            lblHint = new Label();
            btnWord = new Button();
            btnExcel = new Button();
            lblExport = new Label();
            tabs = new TabControl();
            tabRecommendations = new TabPage();
            lblResult = new Label();
            gridRecommendations = new DataGridView();
            lblExercises = new Label();
            gridExercises = new DataGridView();
            tabCatalog = new TabPage();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblFilterGoal = new Label();
            cmbFilterGoal = new ComboBox();
            lblFilterIntensity = new Label();
            cmbFilterIntensity = new ComboBox();
            lblMaxSessions = new Label();
            numMaxSessions = new NumericUpDown();
            lblSort = new Label();
            cmbSort = new ComboBox();
            btnReset = new Button();
            lblCount = new Label();
            gridCatalog = new DataGridView();
            lblCatalogExercises = new Label();
            gridCatalogExercises = new DataGridView();
            profile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSessions).BeginInit();
            tabs.SuspendLayout();
            tabRecommendations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridRecommendations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridExercises).BeginInit();
            tabCatalog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxSessions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridCatalog).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridCatalogExercises).BeginInit();
            SuspendLayout();
            // 
            // profile
            // 
            profile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            profile.Controls.Add(lblUser);
            profile.Controls.Add(cmbUser);
            profile.Controls.Add(btnNew);
            profile.Controls.Add(btnRefresh);
            profile.Controls.Add(lblName);
            profile.Controls.Add(txtName);
            profile.Controls.Add(lblAge);
            profile.Controls.Add(numAge);
            profile.Controls.Add(lblHeight);
            profile.Controls.Add(numHeight);
            profile.Controls.Add(lblWeight);
            profile.Controls.Add(numWeight);
            profile.Controls.Add(lblLevel);
            profile.Controls.Add(cmbLevel);
            profile.Controls.Add(lblGoal);
            profile.Controls.Add(cmbGoal);
            profile.Controls.Add(lblIntensity);
            profile.Controls.Add(cmbIntensity);
            profile.Controls.Add(lblSessions);
            profile.Controls.Add(numSessions);
            profile.Controls.Add(btnSave);
            profile.Controls.Add(btnRecommend);
            profile.Controls.Add(lblHint);
            profile.Location = new Point(14, 13);
            profile.Margin = new Padding(3, 4, 3, 4);
            profile.Name = "profile";
            profile.Padding = new Padding(3, 4, 3, 4);
            profile.Size = new Size(1383, 288);
            profile.TabIndex = 0;
            profile.TabStop = false;
            profile.Text = "Анкета користувача";
            // 
            // lblUser
            // 
            lblUser.Location = new Point(16, 33);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(183, 29);
            lblUser.TabIndex = 0;
            lblUser.Text = "Збережений користувач";
            // 
            // cmbUser
            // 
            cmbUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUser.Location = new Point(206, 29);
            cmbUser.Margin = new Padding(3, 4, 3, 4);
            cmbUser.Name = "cmbUser";
            cmbUser.Size = new Size(342, 28);
            cmbUser.TabIndex = 1;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(560, 27);
            btnNew.Margin = new Padding(3, 4, 3, 4);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(149, 43);
            btnNew.TabIndex = 2;
            btnNew.Text = "Нова анкета";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(720, 27);
            btnRefresh.Margin = new Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(171, 43);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Оновити дані";
            // 
            // lblName
            // 
            lblName.Location = new Point(16, 87);
            lblName.Name = "lblName";
            lblName.Size = new Size(183, 29);
            lblName.TabIndex = 4;
            lblName.Text = "Ім’я";
            // 
            // txtName
            // 
            txtName.Location = new Point(206, 83);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.MaxLength = 100;
            txtName.Name = "txtName";
            txtName.Size = new Size(342, 27);
            txtName.TabIndex = 5;
            // 
            // lblAge
            // 
            lblAge.Location = new Point(571, 87);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(57, 29);
            lblAge.TabIndex = 6;
            lblAge.Text = "Вік";
            // 
            // numAge
            // 
            numAge.Location = new Point(629, 83);
            numAge.Margin = new Padding(3, 4, 3, 4);
            numAge.Minimum = new decimal(new int[] { 18, 0, 0, 0 });
            numAge.Name = "numAge";
            numAge.Size = new Size(86, 27);
            numAge.TabIndex = 7;
            numAge.Value = new decimal(new int[] { 22, 0, 0, 0 });
            // 
            // lblHeight
            // 
            lblHeight.Location = new Point(743, 87);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new Size(103, 29);
            lblHeight.TabIndex = 8;
            lblHeight.Text = "Зріст";
            // 
            // numHeight
            // 
            numHeight.DecimalPlaces = 2;
            numHeight.Location = new Point(846, 83);
            numHeight.Margin = new Padding(3, 4, 3, 4);
            numHeight.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numHeight.Name = "numHeight";
            numHeight.Size = new Size(103, 27);
            numHeight.TabIndex = 9;
            numHeight.Value = new decimal(new int[] { 175, 0, 0, 0 });
            // 
            // lblWeight
            // 
            lblWeight.Location = new Point(971, 87);
            lblWeight.Name = "lblWeight";
            lblWeight.Size = new Size(103, 29);
            lblWeight.TabIndex = 10;
            lblWeight.Text = "Вага";
            // 
            // numWeight
            // 
            numWeight.DecimalPlaces = 2;
            numWeight.Location = new Point(1074, 83);
            numWeight.Margin = new Padding(3, 4, 3, 4);
            numWeight.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numWeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWeight.Name = "numWeight";
            numWeight.Size = new Size(114, 27);
            numWeight.TabIndex = 11;
            numWeight.Value = new decimal(new int[] { 70, 0, 0, 0 });
            // 
            // lblLevel
            // 
            lblLevel.Location = new Point(16, 140);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(183, 29);
            lblLevel.TabIndex = 12;
            lblLevel.Text = "Рівень підготовки";
            // 
            // cmbLevel
            // 
            cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevel.Location = new Point(206, 136);
            cmbLevel.Margin = new Padding(3, 4, 3, 4);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.Size = new Size(251, 28);
            cmbLevel.TabIndex = 13;
            // 
            // lblGoal
            // 
            lblGoal.Location = new Point(480, 140);
            lblGoal.Name = "lblGoal";
            lblGoal.Size = new Size(80, 29);
            lblGoal.TabIndex = 14;
            lblGoal.Text = "Мета";
            // 
            // cmbGoal
            // 
            cmbGoal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGoal.Location = new Point(560, 136);
            cmbGoal.Margin = new Padding(3, 4, 3, 4);
            cmbGoal.Name = "cmbGoal";
            cmbGoal.Size = new Size(274, 28);
            cmbGoal.TabIndex = 15;
            // 
            // lblIntensity
            // 
            lblIntensity.Location = new Point(857, 140);
            lblIntensity.Name = "lblIntensity";
            lblIntensity.Size = new Size(137, 29);
            lblIntensity.TabIndex = 16;
            lblIntensity.Text = "Інтенсивність";
            // 
            // cmbIntensity
            // 
            cmbIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIntensity.Location = new Point(1006, 136);
            cmbIntensity.Margin = new Padding(3, 4, 3, 4);
            cmbIntensity.Name = "cmbIntensity";
            cmbIntensity.Size = new Size(182, 28);
            cmbIntensity.TabIndex = 17;
            // 
            // lblSessions
            // 
            lblSessions.Location = new Point(16, 197);
            lblSessions.Name = "lblSessions";
            lblSessions.Size = new Size(183, 29);
            lblSessions.TabIndex = 18;
            lblSessions.Text = "Занять на тиждень";
            // 
            // numSessions
            // 
            numSessions.Location = new Point(206, 193);
            numSessions.Margin = new Padding(3, 4, 3, 4);
            numSessions.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numSessions.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSessions.Name = "numSessions";
            numSessions.Size = new Size(91, 27);
            numSessions.TabIndex = 19;
            numSessions.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // btnSave
            // 
            btnSave.Location = new Point(331, 188);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(217, 48);
            btnSave.TabIndex = 20;
            btnSave.Text = "Зберегти анкету";
            // 
            // btnRecommend
            // 
            btnRecommend.Location = new Point(560, 188);
            btnRecommend.Margin = new Padding(3, 4, 3, 4);
            btnRecommend.Name = "btnRecommend";
            btnRecommend.Size = new Size(274, 48);
            btnRecommend.TabIndex = 21;
            btnRecommend.Text = "Підібрати 3 плани";
            // 
            // lblHint
            // 
            lblHint.Location = new Point(16, 247);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(1349, 29);
            lblHint.TabIndex = 22;
            lblHint.Click += lblHint_Click;
            // 
            // btnWord
            // 
            btnWord.Location = new Point(14, 315);
            btnWord.Margin = new Padding(3, 4, 3, 4);
            btnWord.Name = "btnWord";
            btnWord.Size = new Size(211, 45);
            btnWord.TabIndex = 1;
            btnWord.Text = "Експорт у Word";
            // 
            // btnExcel
            // 
            btnExcel.Location = new Point(237, 315);
            btnExcel.Margin = new Padding(3, 4, 3, 4);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(211, 45);
            btnExcel.TabIndex = 2;
            btnExcel.Text = "Експорт у Excel";
            // 
            // lblExport
            // 
            lblExport.Location = new Point(469, 323);
            lblExport.Name = "lblExport";
            lblExport.Size = new Size(914, 29);
            lblExport.TabIndex = 3;
            lblExport.Text = "Експорт активної вкладки: усі показані плани та вправи до них.";
            // 
            // tabs
            // 
            tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabs.Controls.Add(tabRecommendations);
            tabs.Controls.Add(tabCatalog);
            tabs.Location = new Point(14, 373);
            tabs.Margin = new Padding(3, 4, 3, 4);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(1383, 614);
            tabs.TabIndex = 4;
            // 
            // tabRecommendations
            // 
            tabRecommendations.Controls.Add(lblResult);
            tabRecommendations.Controls.Add(gridRecommendations);
            tabRecommendations.Controls.Add(lblExercises);
            tabRecommendations.Controls.Add(gridExercises);
            tabRecommendations.Location = new Point(4, 29);
            tabRecommendations.Margin = new Padding(3, 4, 3, 4);
            tabRecommendations.Name = "tabRecommendations";
            tabRecommendations.Size = new Size(1375, 581);
            tabRecommendations.TabIndex = 0;
            tabRecommendations.Text = "Мої 3 плани";
            // 
            // lblResult
            // 
            lblResult.Location = new Point(9, 11);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(1314, 29);
            lblResult.TabIndex = 0;
            lblResult.Text = "Усі планти тренувань";
            lblResult.Click += lblResult_Click;
            // 
            // gridRecommendations
            // 
            gridRecommendations.AllowUserToAddRows = false;
            gridRecommendations.AllowUserToDeleteRows = false;
            gridRecommendations.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gridRecommendations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridRecommendations.ColumnHeadersHeight = 29;
            gridRecommendations.Location = new Point(9, 47);
            gridRecommendations.Margin = new Padding(3, 4, 3, 4);
            gridRecommendations.MultiSelect = false;
            gridRecommendations.Name = "gridRecommendations";
            gridRecommendations.ReadOnly = true;
            gridRecommendations.RowHeadersVisible = false;
            gridRecommendations.RowHeadersWidth = 51;
            gridRecommendations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridRecommendations.Size = new Size(1351, 253);
            gridRecommendations.TabIndex = 1;
            // 
            // lblExercises
            // 
            lblExercises.Location = new Point(9, 313);
            lblExercises.Name = "lblExercises";
            lblExercises.Size = new Size(1314, 29);
            lblExercises.TabIndex = 2;
            lblExercises.Text = "Вправи вибраного плану";
            // 
            // gridExercises
            // 
            gridExercises.AllowUserToAddRows = false;
            gridExercises.AllowUserToDeleteRows = false;
            gridExercises.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridExercises.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridExercises.ColumnHeadersHeight = 29;
            gridExercises.Location = new Point(9, 351);
            gridExercises.Margin = new Padding(3, 4, 3, 4);
            gridExercises.MultiSelect = false;
            gridExercises.Name = "gridExercises";
            gridExercises.ReadOnly = true;
            gridExercises.RowHeadersVisible = false;
            gridExercises.RowHeadersWidth = 51;
            gridExercises.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridExercises.Size = new Size(1351, 202);
            gridExercises.TabIndex = 3;
            // 
            // tabCatalog
            // 
            tabCatalog.Controls.Add(lblSearch);
            tabCatalog.Controls.Add(txtSearch);
            tabCatalog.Controls.Add(lblFilterGoal);
            tabCatalog.Controls.Add(cmbFilterGoal);
            tabCatalog.Controls.Add(lblFilterIntensity);
            tabCatalog.Controls.Add(cmbFilterIntensity);
            tabCatalog.Controls.Add(lblMaxSessions);
            tabCatalog.Controls.Add(numMaxSessions);
            tabCatalog.Controls.Add(lblSort);
            tabCatalog.Controls.Add(cmbSort);
            tabCatalog.Controls.Add(btnReset);
            tabCatalog.Controls.Add(lblCount);
            tabCatalog.Controls.Add(gridCatalog);
            tabCatalog.Controls.Add(lblCatalogExercises);
            tabCatalog.Controls.Add(gridCatalogExercises);
            tabCatalog.Location = new Point(4, 29);
            tabCatalog.Margin = new Padding(3, 4, 3, 4);
            tabCatalog.Name = "tabCatalog";
            tabCatalog.Size = new Size(1375, 581);
            tabCatalog.TabIndex = 1;
            tabCatalog.Text = "Каталог: пошук і фільтри";
            // 
            // lblSearch
            // 
            lblSearch.Location = new Point(9, 13);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(80, 29);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Пошук";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(91, 9);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(308, 27);
            txtSearch.TabIndex = 1;
            // 
            // lblFilterGoal
            // 
            lblFilterGoal.Location = new Point(417, 13);
            lblFilterGoal.Name = "lblFilterGoal";
            lblFilterGoal.Size = new Size(69, 29);
            lblFilterGoal.TabIndex = 2;
            lblFilterGoal.Text = "Мета";
            // 
            // cmbFilterGoal
            // 
            cmbFilterGoal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterGoal.Location = new Point(486, 9);
            cmbFilterGoal.Margin = new Padding(3, 4, 3, 4);
            cmbFilterGoal.Name = "cmbFilterGoal";
            cmbFilterGoal.Size = new Size(251, 28);
            cmbFilterGoal.TabIndex = 3;
            // 
            // lblFilterIntensity
            // 
            lblFilterIntensity.Location = new Point(760, 13);
            lblFilterIntensity.Name = "lblFilterIntensity";
            lblFilterIntensity.Size = new Size(131, 29);
            lblFilterIntensity.TabIndex = 4;
            lblFilterIntensity.Text = "Інтенсивність";
            // 
            // cmbFilterIntensity
            // 
            cmbFilterIntensity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterIntensity.Location = new Point(897, 9);
            cmbFilterIntensity.Margin = new Padding(3, 4, 3, 4);
            cmbFilterIntensity.Name = "cmbFilterIntensity";
            cmbFilterIntensity.Size = new Size(171, 28);
            cmbFilterIntensity.TabIndex = 5;
            // 
            // lblMaxSessions
            // 
            lblMaxSessions.Location = new Point(1086, 13);
            lblMaxSessions.Name = "lblMaxSessions";
            lblMaxSessions.Size = new Size(171, 29);
            lblMaxSessions.TabIndex = 6;
            lblMaxSessions.Text = "Макс. занять/тиж.";
            // 
            // numMaxSessions
            // 
            numMaxSessions.Location = new Point(1257, 9);
            numMaxSessions.Margin = new Padding(3, 4, 3, 4);
            numMaxSessions.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numMaxSessions.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxSessions.Name = "numMaxSessions";
            numMaxSessions.Size = new Size(91, 27);
            numMaxSessions.TabIndex = 7;
            numMaxSessions.Value = new decimal(new int[] { 7, 0, 0, 0 });
            // 
            // lblSort
            // 
            lblSort.Location = new Point(9, 67);
            lblSort.Name = "lblSort";
            lblSort.Size = new Size(114, 29);
            lblSort.TabIndex = 8;
            lblSort.Text = "Сортування";
            // 
            // cmbSort
            // 
            cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSort.Location = new Point(126, 63);
            cmbSort.Margin = new Padding(3, 4, 3, 4);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new Size(342, 28);
            cmbSort.TabIndex = 9;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(491, 60);
            btnReset.Margin = new Padding(3, 4, 3, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(183, 43);
            btnReset.TabIndex = 10;
            btnReset.Text = "Скинути фільтри";
            // 
            // lblCount
            // 
            lblCount.Location = new Point(697, 67);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(629, 29);
            lblCount.TabIndex = 11;
            lblCount.Text = "Знайдено планів: 0";
            // 
            // gridCatalog
            // 
            gridCatalog.AllowUserToAddRows = false;
            gridCatalog.AllowUserToDeleteRows = false;
            gridCatalog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gridCatalog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridCatalog.ColumnHeadersHeight = 29;
            gridCatalog.Location = new Point(9, 120);
            gridCatalog.Margin = new Padding(3, 4, 3, 4);
            gridCatalog.MultiSelect = false;
            gridCatalog.Name = "gridCatalog";
            gridCatalog.ReadOnly = true;
            gridCatalog.RowHeadersVisible = false;
            gridCatalog.RowHeadersWidth = 51;
            gridCatalog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridCatalog.Size = new Size(1351, 227);
            gridCatalog.TabIndex = 12;
            // 
            // lblCatalogExercises
            // 
            lblCatalogExercises.Location = new Point(9, 360);
            lblCatalogExercises.Name = "lblCatalogExercises";
            lblCatalogExercises.Size = new Size(1314, 29);
            lblCatalogExercises.TabIndex = 13;
            lblCatalogExercises.Text = "Вправи вибраного плану";
            // 
            // gridCatalogExercises
            // 
            gridCatalogExercises.AllowUserToAddRows = false;
            gridCatalogExercises.AllowUserToDeleteRows = false;
            gridCatalogExercises.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridCatalogExercises.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridCatalogExercises.ColumnHeadersHeight = 29;
            gridCatalogExercises.Location = new Point(9, 397);
            gridCatalogExercises.Margin = new Padding(3, 4, 3, 4);
            gridCatalogExercises.MultiSelect = false;
            gridCatalogExercises.Name = "gridCatalogExercises";
            gridCatalogExercises.ReadOnly = true;
            gridCatalogExercises.RowHeadersVisible = false;
            gridCatalogExercises.RowHeadersWidth = 51;
            gridCatalogExercises.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridCatalogExercises.Size = new Size(1351, 156);
            gridCatalogExercises.TabIndex = 14;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(1234, 851);
            ClientSize = new Size(1410, 1055);
            Controls.Add(profile);
            Controls.Add(btnWord);
            Controls.Add(btnExcel);
            Controls.Add(lblExport);
            Controls.Add(tabs);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1140, 918);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            profile.ResumeLayout(false);
            profile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSessions).EndInit();
            tabs.ResumeLayout(false);
            tabRecommendations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridRecommendations).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridExercises).EndInit();
            tabCatalog.ResumeLayout(false);
            tabCatalog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxSessions).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridCatalog).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridCatalogExercises).EndInit();
            ResumeLayout(false);
        }

        private GroupBox profile;
        private Label lblUser;
        private ComboBox cmbUser;
        private Button btnNew;
        private Button btnRefresh;
        private Label lblName;
        private TextBox txtName;
        private Label lblAge;
        private NumericUpDown numAge;
        private Label lblHeight;
        private NumericUpDown numHeight;
        private Label lblWeight;
        private NumericUpDown numWeight;
        private Label lblLevel;
        private ComboBox cmbLevel;
        private Label lblGoal;
        private ComboBox cmbGoal;
        private Label lblIntensity;
        private ComboBox cmbIntensity;
        private Label lblSessions;
        private NumericUpDown numSessions;
        private Button btnSave;
        private Button btnRecommend;
        private Label lblHint;
        private Button btnWord;
        private Button btnExcel;
        private Label lblExport;
        private TabControl tabs;
        private TabPage tabRecommendations;
        private DataGridView gridRecommendations;
        private Label lblExercises;
        private DataGridView gridExercises;
        private TabPage tabCatalog;
        private Label lblSearch;
        private TextBox txtSearch;
        private Label lblFilterGoal;
        private ComboBox cmbFilterGoal;
        private Label lblFilterIntensity;
        private ComboBox cmbFilterIntensity;
        private Label lblMaxSessions;
        private NumericUpDown numMaxSessions;
        private Label lblSort;
        private ComboBox cmbSort;
        private Button btnReset;
        private Label lblCount;
        private DataGridView gridCatalog;
        private Label lblCatalogExercises;
        private DataGridView gridCatalogExercises;
        private Label lblResult;
    }
}
