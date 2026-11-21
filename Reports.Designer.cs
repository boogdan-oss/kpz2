using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Reporting.WinForms;
using System.Drawing;
using System.Windows.Forms;
using Font = System.Drawing.Font;
using CheckBox = System.Windows.Forms.CheckBox;

namespace kpz_lr3.Reports
{
    partial class Reports
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbKind;
        private ComboBox cmbUser;
        private CheckBox chkPeriod;
        private DateTimePicker dateFrom;
        private DateTimePicker dateTo;
        private Button btnBuild;
        private Button btnPdf;
        private Button btnClose;
        private Label lblKind;
        private Label lblUser;
        private Label lblFrom;
        private Label lblTo;
        private ReportViewer viewer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            cmbKind = new ComboBox();
            cmbUser = new ComboBox();
            chkPeriod = new CheckBox();
            dateFrom = new DateTimePicker();
            dateTo = new DateTimePicker();
            btnBuild = new Button();
            btnPdf = new Button();
            btnClose = new Button();
            lblKind = new Label();
            lblUser = new Label();
            lblFrom = new Label();
            lblTo = new Label();
            viewer = new ReportViewer();
            SuspendLayout();
            // 
            // cmbKind
            // 
            cmbKind.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKind.DropDownWidth = 600;
            cmbKind.Location = new Point(12, 36);
            cmbKind.Name = "cmbKind";
            cmbKind.Size = new Size(510, 31);
            cmbKind.TabIndex = 1;
            // 
            // cmbUser
            // 
            cmbUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUser.Location = new Point(540, 36);
            cmbUser.Name = "cmbUser";
            cmbUser.Size = new Size(430, 31);
            cmbUser.TabIndex = 3;
            // 
            // chkPeriod
            // 
            chkPeriod.Location = new Point(12, 99);
            chkPeriod.Name = "chkPeriod";
            chkPeriod.Size = new Size(125, 30);
            chkPeriod.TabIndex = 5;
            chkPeriod.Text = "За період";
            // 
            // dateFrom
            // 
            dateFrom.Format = DateTimePickerFormat.Short;
            dateFrom.Location = new Point(151, 102);
            dateFrom.MaxDate = new DateTime(9998, 12, 30, 0, 0, 0, 0);
            dateFrom.Name = "dateFrom";
            dateFrom.Size = new Size(170, 30);
            dateFrom.TabIndex = 7;
            dateFrom.Value = new DateTime(2026, 10, 1, 0, 0, 0, 0);
            // 
            // dateTo
            // 
            dateTo.Format = DateTimePickerFormat.Short;
            dateTo.Location = new Point(337, 102);
            dateTo.MaxDate = new DateTime(9998, 12, 30, 0, 0, 0, 0);
            dateTo.Name = "dateTo";
            dateTo.Size = new Size(170, 30);
            dateTo.TabIndex = 9;
            dateTo.Value = new DateTime(2026, 10, 23, 0, 0, 0, 0);
            // 
            // btnBuild
            // 
            btnBuild.Location = new Point(540, 99);
            btnBuild.Name = "btnBuild";
            btnBuild.Size = new Size(170, 36);
            btnBuild.TabIndex = 10;
            btnBuild.Text = "Сформувати";
            // 
            // btnPdf
            // 
            btnPdf.Enabled = false;
            btnPdf.Location = new Point(722, 99);
            btnPdf.Name = "btnPdf";
            btnPdf.Size = new Size(170, 36);
            btnPdf.TabIndex = 11;
            btnPdf.Text = "Зберегти PDF";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(994, 34);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(214, 34);
            btnClose.TabIndex = 4;
            btnClose.Text = "Назад";
            // 
            // lblKind
            // 
            lblKind.Location = new Point(12, 10);
            lblKind.Name = "lblKind";
            lblKind.Size = new Size(200, 24);
            lblKind.TabIndex = 0;
            lblKind.Text = "Вид звіту";
            // 
            // lblUser
            // 
            lblUser.Location = new Point(540, 10);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(410, 24);
            lblUser.TabIndex = 2;
            lblUser.Text = "Користувач (для третього звіту)";
            // 
            // lblFrom
            // 
            lblFrom.Location = new Point(151, 78);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(80, 24);
            lblFrom.TabIndex = 6;
            lblFrom.Text = "Від";
            // 
            // lblTo
            // 
            lblTo.Location = new Point(337, 78);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(170, 24);
            lblTo.TabIndex = 8;
            lblTo.Text = "До (включно)";
            // 
            // viewer
            // 
            viewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            viewer.Location = new Point(12, 151);
            viewer.Name = "viewer";
            viewer.ServerReport.BearerToken = null;
            viewer.Size = new Size(1196, 550);
            viewer.TabIndex = 12;
            // 
            // Reports
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1222, 760);
            Controls.Add(lblKind);
            Controls.Add(cmbKind);
            Controls.Add(lblUser);
            Controls.Add(cmbUser);
            Controls.Add(btnClose);
            Controls.Add(chkPeriod);
            Controls.Add(lblFrom);
            Controls.Add(dateFrom);
            Controls.Add(lblTo);
            Controls.Add(dateTo);
            Controls.Add(btnBuild);
            Controls.Add(btnPdf);
            Controls.Add(viewer);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1240, 650);
            Name = "Reports";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Звіти та діаграми RDLC — My db";
            ResumeLayout(false);
        }
    }
}
