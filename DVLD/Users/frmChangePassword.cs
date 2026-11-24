using Business_Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID = -1;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            _UserID = UserID;
            InitializeComponent();
        }

        private void frmChangePasswordForm1_Load(object sender, EventArgs e)
        {
            if (ctrlUserCard1.FillCardWithData(_UserID))
            {
                _ResetDefualtValues();
                return;
            }
        }

        private void btnShowHide_Click(object sender, EventArgs e)
        {
            if (tbNewPassword.UseSystemPasswordChar == true)
            {
                tbCurrentPassword.UseSystemPasswordChar = false;
                tbConfirmPassword.UseSystemPasswordChar = false;
                tbNewPassword.UseSystemPasswordChar = false;
                btnShowHide.Text = "🔒";
            }
            else
            {
                tbCurrentPassword.UseSystemPasswordChar = true;
                tbConfirmPassword.UseSystemPasswordChar = true;
                tbNewPassword.UseSystemPasswordChar = true;
                btnShowHide.Text = "👁";
            }
        }

        private void _ResetDefualtValues()
        {
            tbCurrentPassword.Text = "";
            tbNewPassword.Text = "";
            tbConfirmPassword.Text = "";
            tbCurrentPassword.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Password doesn't Changed, There Is an Error on Fields", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (clsUser.ChangePassword(_UserID, tbCurrentPassword.Text.Trim(), tbNewPassword.Text.Trim()))
            {
                case clsUser.enChangePasswordResult.Success:
                    MessageBox.Show("Password Changed Successfully");
                    _ResetDefualtValues();
                    break;

                case clsUser.enChangePasswordResult.EmptyFields:
                    MessageBox.Show("Please Fill All Fields");
                    break;

                case clsUser.enChangePasswordResult.UserNotFound:
                    MessageBox.Show("User Not Found");
                    break;

                case clsUser.enChangePasswordResult.WrongOldPassword:
                    MessageBox.Show("Old Password Is Incorrect");
                    break;

                case clsUser.enChangePasswordResult.DatabaseError:
                    MessageBox.Show("Failed To Change Password");
                    break;
            }
        }

        private void tb_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                e.Cancel = true;
                //((TextBox)sender).Focus();
                errorProvider1.SetError(((TextBox)sender), "Please Insert a Value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(((TextBox)sender), "");
            }
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword.Text.Trim() != tbConfirmPassword.Text.Trim())
            {
                errorProvider1.SetError(tbConfirmPassword, "Please Confirm Password");
            }
            else
            {
                errorProvider1.SetError(tbConfirmPassword, "");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
