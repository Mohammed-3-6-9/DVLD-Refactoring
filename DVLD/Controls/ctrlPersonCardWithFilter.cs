using Business_Logic;
using DVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public clsPerson SelectedPerson { get => ctrlPersonCard1.Person; }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get => _FilterEnabled;
            set => _FilterEnabled = gbFilter.Enabled = value;
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get => _ShowAddPerson;
            set => _ShowAddPerson = btnAddPerson.Visible = value;
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            cbFilter.SelectedItem = "Person ID";
        }

        void IDSentBack(object sender, int PersonID)
        {
            cbFilter.SelectedItem = "Person ID";
            tbFilterValue.Text = PersonID.ToString();
            int result = ctrlPersonCard1.FillCardWithData(PersonID);
            OnPersonSelected?.Invoke(result);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.SendIDBackEvent += IDSentBack;
            frm.ShowDialog();
        }

        private void FindPersonInfo()
        {
            if (string.IsNullOrWhiteSpace(tbFilterValue.Text))
            {
                errorProvider1.SetError(tbFilterValue, "Please Insert a Value");
                return;
            }
            else
                errorProvider1.SetError(tbFilterValue, string.Empty);


            if (cbFilter.SelectedItem.ToString() == "Person ID")
            {
                if (!int.TryParse(tbFilterValue.Text, out int personID))
                {
                    errorProvider1.SetError(tbFilterValue, "Please Insert a Valid Value");
                    return;
                }
                else
                {
                    ctrlPersonCard1.FillCardWithData(personID);
                    errorProvider1.SetError(tbFilterValue, string.Empty);
                }
            }
            else
            {
                ctrlPersonCard1.FillCardWithData(tbFilterValue.Text);
            }

            if (_FilterEnabled)
                OnPersonSelected?.Invoke(ctrlPersonCard1.PersonID);
        }

        public void FindPersonInfo(int PersonID)
        {
            cbFilter.SelectedIndex = 0;
            tbFilterValue.Text = PersonID.ToString();
            FindPersonInfo();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FindPersonInfo();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilterValue.Text = "";
            tbFilterValue.Focus();
            errorProvider1.SetError(tbFilterValue, string.Empty);
        }

        private void tbFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }

            if (cbFilter.Text == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    errorProvider1.SetError(((TextBox)sender), "Please Insert a Number");
                    e.Handled = true;
                }
                else
                    errorProvider1.SetError((TextBox)sender, string.Empty);
            }
        }
    }
}
