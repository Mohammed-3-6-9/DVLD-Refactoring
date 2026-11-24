using Business_Logic;
using DVLD.People;
using DVLD.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD
{
    public partial class frmUsers : Form
    {
        private DataView _DataView;

        public frmUsers()
        {
            InitializeComponent();
        }

        private void _RefreshUsers()
        {
            DataTable dt = clsUser.GetAllUsers();

            _DataView = dt.DefaultView;
            dgvUsers.DataSource = _DataView;
            cbFiltersType.SelectedIndex = 0;

            lblRecordsNumber.Text = dgvUsers.RowCount.ToString();
        }
        private void frmUsers_Load(object sender, EventArgs e)
        {
            _RefreshUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails frm = new frmUserDetails((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void tbFilterValue_TextChanged(object sender, EventArgs e)
        {
            if(cbFiltersType.Text == "None" || string.IsNullOrWhiteSpace(tbFilterValue.Text))
            {
                _DataView.RowFilter = null;
                lblRecordsNumber.Text=dgvUsers.RowCount.ToString();
                return;
            }

            if (cbFiltersType.Text == "PersonID" || cbFiltersType.Text == "UserID")
            {
                _DataView.RowFilter = $"{cbFiltersType.Text} = {tbFilterValue.Text}";
            }
            else
            {
                string Filter = tbFilterValue.Text.Trim().Replace("'", "''");
                _DataView.RowFilter = $"{cbFiltersType.Text} LIKE '%{Filter}%'";
            }

            lblRecordsNumber.Text = dgvUsers.RowCount.ToString();
        }

        private void cbFiltersType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbFiltersType.Text == "IsActive")
            {
                cbIsActive.Visible = true;
                tbFilterValue.Visible = false;
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
            }
            else
            {
                tbFilterValue.Visible = (cbFiltersType.Text != "None");
                cbIsActive.Visible = false;
                tbFilterValue.Text = "";
                tbFilterValue.Focus();
            }
        }

        private void tbFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFiltersType.Text == "PersonID" || cbFiltersType.Text == "UserID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        void _MessageFeatureNotImplemented()
        {
            MessageBox.Show($"Sorry This Feature Not Implemented Yet");
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _MessageFeatureNotImplemented();
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _MessageFeatureNotImplemented();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string choice = cbIsActive.SelectedItem.ToString();

            if (choice == "Active")
            {
                _DataView.RowFilter = $"IsActive = {1}";
            }
            else if (choice == "Not Active")
            {
                _DataView.RowFilter = $"IsActive = {0}";
            }
            else
                _DataView.RowFilter = null;
        }

        void DataUpdated()
        {
            _RefreshUsers();
        }
        private void btnAddPerson_Click_1(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.DataUpdatedEvent += DataUpdated;
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.DataUpdatedEvent += DataUpdated;
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser((int)dgvUsers.CurrentRow.Cells["UserID"].Value);
            frm.DataUpdatedEvent += DataUpdated;
            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = int.Parse(dgvUsers.CurrentRow.Cells["UserID"].Value.ToString());
            if (MessageBox.Show($"Are you sure you want to delete User [ {UserID.ToString()} ]",
                "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsUser.DeleteUser(UserID))
                {
                    MessageBox.Show("User Deleted Successfully");
                    _RefreshUsers();
                }
                else
                    MessageBox.Show("User is not deleted, It is Connected to other Entities");
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(
                int.Parse(dgvUsers.CurrentRow.Cells["UserID"].Value.ToString()));
            frm.ShowDialog();
        }
    }
}
