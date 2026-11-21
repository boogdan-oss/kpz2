using System.Drawing;
using System.Windows.Forms;

namespace kpz_lr3
{
    partial class Admin
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbTable;
        private ComboBox cmbReport;
        private TextBox txtSearch;
        private TextBox txtSql;
        private NumericUpDown numId;
        private DataGridView grid;
        private TableLayoutPanel fields;
        private Button btnNew;
        private Button btnInsert;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnSearch;
        private Button btnReport;
        private Button btnBack;
        private Label lblStatus;
        private Label lblTableTitle;
        private Label lblReportTitle;
        private Label lblIdTitle;
        private Label lblEditorTitle;
        private Label lblSqlTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmbTable = new ComboBox();
            cmbReport = new ComboBox();
            txtSearch = new TextBox();
            txtSql = new TextBox();
            numId = new NumericUpDown();
            grid = new DataGridView();
            fields = new TableLayoutPanel();
            btnNew = new Button();
            btnInsert = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnSearch = new Button();
            btnReport = new Button();
            btnBack = new Button();
            lblStatus = new Label();
            lblTableTitle = new Label();
            lblReportTitle = new Label();
            lblIdTitle = new Label();
            lblEditorTitle = new Label();
            lblSqlTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numId).BeginInit();
            SuspendLayout();

            lblTableTitle.Location = new Point(12, 10);
            lblTableTitle.Size = new Size(170, 24);
            lblTableTitle.Text = "Таблиця";
            cmbTable.Name = "cmbTable";
            cmbTable.Location = new Point(12, 36);
            cmbTable.Size = new Size(175, 30);
            cmbTable.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTable.TabIndex = 0;
            txtSearch.Name = "txtSearch";
            txtSearch.Location = new Point(199, 36);
            txtSearch.Size = new Size(375, 30);
            txtSearch.PlaceholderText = "Пошук у вибраній таблиці";
            txtSearch.TabIndex = 1;
            btnSearch.Name = "btnSearch";
            btnSearch.Location = new Point(586, 34);
            btnSearch.Size = new Size(180, 34);
            btnSearch.Text = "Знайти / оновити";
            btnSearch.TabIndex = 2;
            btnBack.Name = "btnBack";
            btnBack.Location = new Point(980, 34);
            btnBack.Size = new Size(228, 34);
            btnBack.Text = "Назад до користувача";
            btnBack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBack.TabIndex = 3;

            lblReportTitle.Location = new Point(12, 78);
            lblReportTitle.Size = new Size(500, 24);
            lblReportTitle.Text = "Запити, підзапити та процедури";
            cmbReport.Name = "cmbReport";
            cmbReport.Location = new Point(12, 104);
            cmbReport.Size = new Size(562, 30);
            cmbReport.DropDownWidth = 650;
            cmbReport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReport.TabIndex = 4;
            lblIdTitle.Location = new Point(586, 78);
            lblIdTitle.Size = new Size(110, 24);
            lblIdTitle.Text = "ID / мета";
            numId.Name = "numId";
            numId.Location = new Point(586, 104);
            numId.Size = new Size(100, 30);
            numId.Maximum = 2147483647M;
            numId.Value = 1M;
            numId.TabIndex = 5;
            btnReport.Name = "btnReport";
            btnReport.Location = new Point(698, 102);
            btnReport.Size = new Size(180, 34);
            btnReport.Text = "Виконати запит";
            btnReport.TabIndex = 6;

            grid.Name = "grid";
            grid.Location = new Point(12, 153);
            grid.Size = new Size(820, 410);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.RowHeadersVisible = false;
            grid.TabIndex = 7;

            lblEditorTitle.Location = new Point(846, 153);
            lblEditorTitle.Size = new Size(362, 24);
            lblEditorTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEditorTitle.Text = "Поля вибраного запису";
            fields.Name = "fields";
            fields.Location = new Point(846, 184);
            fields.Size = new Size(362, 291);
            fields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            fields.ColumnCount = 2;
            fields.RowCount = 0;
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            fields.AutoScroll = true;
            fields.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            fields.Padding = new Padding(4);
            fields.BackColor = Color.White;
            fields.TabIndex = 8;

            btnNew.Name = "btnNew";
            btnNew.Location = new Point(846, 487);
            btnNew.Size = new Size(175, 34);
            btnNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNew.Text = "Очистити поля";
            btnNew.TabIndex = 9;
            btnInsert.Name = "btnInsert";
            btnInsert.Location = new Point(1033, 487);
            btnInsert.Size = new Size(175, 34);
            btnInsert.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnInsert.Text = "Додати";
            btnInsert.TabIndex = 10;
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Location = new Point(846, 529);
            btnUpdate.Size = new Size(175, 34);
            btnUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnUpdate.Text = "Змінити";
            btnUpdate.TabIndex = 11;
            btnDelete.Name = "btnDelete";
            btnDelete.Location = new Point(1033, 529);
            btnDelete.Size = new Size(175, 34);
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDelete.Text = "Видалити";
            btnDelete.TabIndex = 12;

            lblSqlTitle.Location = new Point(12, 575);
            lblSqlTitle.Size = new Size(1196, 24);
            lblSqlTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblSqlTitle.Text = "Виконаний SQL і параметри";
            txtSql.Name = "txtSql";
            txtSql.Location = new Point(12, 603);
            txtSql.Size = new Size(1196, 100);
            txtSql.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSql.Multiline = true;
            txtSql.ReadOnly = true;
            txtSql.ScrollBars = ScrollBars.Both;
            txtSql.WordWrap = false;
            txtSql.Font = new Font("Consolas", 10F);
            txtSql.TabIndex = 13;
            lblStatus.Name = "lblStatus";
            lblStatus.Location = new Point(12, 712);
            lblStatus.Size = new Size(1196, 38);
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.Text = "Оберіть таблицю. Сортування — клацніть заголовок стовпця.";

            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1220, 760);
            MinimumSize = new Size(1080, 660);
            Font = new Font("Segoe UI", 10F);
            Name = "Admin";
            Text = "Адмінка — My db";
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(lblTableTitle);
            Controls.Add(cmbTable);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(btnBack);
            Controls.Add(lblReportTitle);
            Controls.Add(cmbReport);
            Controls.Add(lblIdTitle);
            Controls.Add(numId);
            Controls.Add(btnReport);
            Controls.Add(grid);
            Controls.Add(lblEditorTitle);
            Controls.Add(fields);
            Controls.Add(btnNew);
            Controls.Add(btnInsert);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(lblSqlTitle);
            Controls.Add(txtSql);
            Controls.Add(lblStatus);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)numId).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
