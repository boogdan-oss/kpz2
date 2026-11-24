using System;
using System.Windows.Forms;

namespace kpz_lr3
{
    public partial class Login : Form
    {
        private const string CorrectLogin = "test";
        private const string CorrectPassword = "1";

        public bool logged_in = false;   

        public Login()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            button2.Click += button2_Click;   
            AcceptButton = button1;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == CorrectLogin && textBox2.Text == CorrectPassword)
            {
                logged_in = true;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(this, "Невірний логін або пароль.", "Помилка входу",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Clear();
                textBox2.Focus();
            }
        }

        private void button2_Click(object? sender, EventArgs e)
        {
            logged_in = false;
            DialogResult = DialogResult.OK;
        }
    }
}