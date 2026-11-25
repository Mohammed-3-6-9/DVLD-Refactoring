using Business_Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD
{
    public partial class frmLogin : Form
    {
        private string _DestinationFile = Path.Combine(Application.StartupPath, "RememberMe.txt");
        private string _Delim = "#//#";
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnShowHide_Click(object sender, EventArgs e)
        {
            if (tbPassword.UseSystemPasswordChar == true)
            {
                tbPassword.UseSystemPasswordChar = false;
                btnShowHide.Text = "🔒";
            }
            else
            {
                tbPassword.UseSystemPasswordChar = true;
                btnShowHide.Text = "👁";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool _Validation()
        {
            bool IsValid = true;

            if (string.IsNullOrWhiteSpace(tbUserName.Text))
            {
                errorProvider1.SetError(tbUserName, "Please Insert a Value");
                IsValid = false;
            }
            else
                errorProvider1.SetError(tbUserName, "");

            if (string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                errorProvider1.SetError(tbPassword, "Please Insert a Value");
                IsValid = false;
            }
            else
                errorProvider1.SetError(tbPassword, "");

            return IsValid;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!_Validation())
                return;

            clsUser user = clsUser.FindbyUserNameAndPassword(tbUserName.Text, tbPassword.Text);

            if(user == null)
            {
                MessageBox.Show("Username or Password Is NOT correct","Login Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            else 
            {
                if (user.IsActive)
                {
                    _RememberUsernameAndPassword();
                    clsSessionInfo.CurrentUser = user;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Your Account Isn't Active, Call Your Admin", "Login Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void _GetStoredCredentials()
        {
            if (!System.IO.File.Exists(_DestinationFile))
            {
                return;
            }

            string Line = "";
            try
            {
                Line = File.ReadAllText(_DestinationFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            string[] Fields = Line.Split(_Delim.ToCharArray());
            if (Fields.Length == 2)
            {
                tbUserName.Text = Fields[0];
                tbPassword.Text = clsUser.DecryptPassword(Fields[1]);
                chkRememberMe.Checked = true;
            }
            else
            {
                MessageBox.Show("Can't Remember The Credentials.");
            }
        }

        void _RememberUsernameAndPassword()
        {
            if (chkRememberMe.Checked)
            {
                string Line = $"{tbUserName.Text.Trim()}{_Delim}{clsUser.EncryptPassword(tbPassword.Text)}";
                try
                {
                    File.WriteAllText(_DestinationFile, Line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                chkRememberMe.Checked = true;
            }
            else
            {
                if (File.Exists(_DestinationFile))
                {
                    try
                    {
                        File.Delete(_DestinationFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _GetStoredCredentials();
        }
    }
}
