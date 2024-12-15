using System;
using System.Windows.Forms;

namespace NEAFormsApplication
{
    public partial class Form1 : Form
    {
        public bool isLoggedIn = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenLoginForm()
        {
            Form7 loginForm = new Form7();
            loginForm.Owner = this;
            loginForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isLoggedIn)
            {
                OpenLoginForm();
                if (isLoggedIn)
                {
                    Form2 f2 = new Form2();
                    f2.Show();
                    this.Hide();
                }
                else
                {
                    this.Hide();
                    MessageBox.Show("Please log in first.");
                }
            }
            else
            {
                Form2 f2 = new Form2();
                f2.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}