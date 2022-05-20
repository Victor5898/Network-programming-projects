using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailClient
{
    public partial class Form2
    {
        Form1 mainForm;
        public bool IsLogin = false;
        string email;
        string password;


        public Form2(Form callingForm)
        {
            InitializeComponent();
            mainForm = callingForm as Form1;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            Login();

            if (email == null || password == null || email == "" || password == "")
            {
                MessageBox.Show("Please introduce email and password!", "Error");
            }
        }

        public void Login()
        {
            email = EmailTextBox.Text;
            password = PasswordTextBox.Text;

            if (email != null && password != null && email != "" && password != "")
            {
                IsLogin = true;
                mainForm.LoginBtn.Text = "Logout";
                mainForm.emailLabel.Visible = true;
                mainForm.userEmail.Visible = true;
                mainForm.userEmail.Text = EmailTextBox.Text;
                Hide();
            }
        }

        public void Logout()
        {
            IsLogin = false;
            PasswordTextBox.Text = null;
            EmailTextBox.Text = null;
            mainForm.emailLabel.Visible = false;
            mainForm.userEmail.Visible = false;
            mainForm.LoginBtn.Text = "Login";
        }
    }
}
