using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kpz_lr3
{
    public partial class Login : Form
    {
        private const string CorrectLogin = "test";
        private const string CorrectPassword = "1";

        public Login()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;   // пароль зірочками
            button1.Click += button1_Click;
            AcceptButton = button1;                  // Enter = вхід
            StartPosition = FormStartPosition.CenterScreen;
        }

     

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == CorrectLogin && textBox2.Text == CorrectPassword)
            {
                DialogResult = DialogResult.OK;   // закриває форму і повідомляє Program.cs про успіх
            }
            else
            {
                MessageBox.Show(this, "Невірний логін або пароль.", "Помилка входу",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Clear();
                textBox2.Focus();
            }
        }
    }
}
