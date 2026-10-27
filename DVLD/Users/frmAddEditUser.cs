using Business_Logic;
using DVLD.Controls;
using DVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmAddEditUser : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        private int _UserID;
        private int _PersonID;
        private clsUser _User;

        public delegate void DataUpdated();
        public event DataUpdated DataUpdatedEvent;

        public frmAddEditUser(int UserID, int PersonID)
        {
            InitializeComponent();

            _UserID = UserID;
            _PersonID= PersonID;
            _Mode = enMode.Update;
        }

        public frmAddEditUser()
        {
            InitializeComponent();

            _UserID = -1;
            _PersonID = -1;
            _Mode = enMode.AddNew;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        void _FillFieldsByUserData()
        {
            _User = clsUser.Find(_UserID);

            if (_User == null)
            {
                MessageBox.Show("This form will be closed because No User with ID = " + _UserID);
                this.Close();
                return;
            }

            lblUserID.Text = _UserID.ToString();
            tbUserName.Text = _User.UserName;
            tbPassword.Text = _User.Password;
            tbConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            tbPassword.Enabled = tbConfirmPassword.Enabled = false;
        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                _FillFieldsByUserData();
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                ctrlPersonCardWithFilter1.FindPersonInfo(_PersonID);
                lblHeader.Text = "Update User";
            }
            else
            {
                btnChangePassword.Enabled = false;
                _User = new clsUser();
                lblHeader.Text = "Add New User";
            }
        }

        void _FillUserWithData()
        {
            _User.UserName = tbUserName.Text.Trim();
            _User.PersonID = _PersonID;
            _User.IsActive = chkIsActive.Checked;
            _User.Password = tbPassword.Text;
        }

        private bool _Validation()
        {
            if (_PersonID == -1)
            {
                MessageBox.Show("Please Set a Person", "Missed Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                tabControl1.SelectedIndex = 0;
                return false;
            }

            string username = tbUserName.Text.Trim();

            if (_Mode == enMode.AddNew)
            {
                if (clsUser.IsUserNameExist(username))
                {
                    errorProvider1.SetError(tbUserName, "UserName Is Already Exist");
                    return false;
                }
                else
                    errorProvider1.SetError(tbUserName, string.Empty);
            }
            else
            {
                if(username !=_User.UserName)
                {
                    if (clsUser.IsUserNameExist(username))
                    {
                        errorProvider1.SetError(tbUserName, "UserName Is Already Exist");
                        return false;
                    }
                }

                errorProvider1.SetError(tbUserName, string.Empty);
            }

            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                return false;
            }

            return true;
        }

        private void _SetUpdateMode()
        {
            tbPassword.Enabled = tbConfirmPassword.Enabled = false;
            btnChangePassword.Enabled = true;
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            lblHeader.Text = "Update User";
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

            if (!_Validation())
                return;

            _FillUserWithData();

            if (_User.Save())
            {
                MessageBox.Show("User Saved Successfully", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
                _SetUpdateMode();
                _UserID = _User.UserID;
                lblUserID.Text = _UserID.ToString();
                DataUpdatedEvent?.Invoke();
            }
            else
                MessageBox.Show("User Didn't Saved", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tb, "Please Insert a Value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tb, "");
            }
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                errorProvider1.SetError(tbConfirmPassword, "Password confirmation does not match password.");
            }
            else
            {
                errorProvider1.SetError(tbConfirmPassword, "");
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(_UserID);
            frm.ShowDialog();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;

            if (_PersonID == -1)
                return;

            if (clsUser.IsUserExistByPersonID(_PersonID))
            {
                _PersonID = -1;

                MessageBox.Show("Person is Already a User", "Ops",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
    }
}
