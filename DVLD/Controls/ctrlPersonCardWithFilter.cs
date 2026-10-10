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
        private int _PersonID { get; set; }

        public delegate void DataUpdated();
        public event DataUpdated DataUpdatedEvent;

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
        }

        void IDSentBack(object sender, int PersonID)
        {
            _PersonID = PersonID;
            ctrlPersonCard1.FillCardWithData(_PersonID);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.SendIDBackEvent += IDSentBack;
            frm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFilterValue.Text))
            {
                return;
            }

            if (cbFilter.SelectedItem.ToString() == "PersonID")
            {
                _PersonID = int.Parse(tbFilterValue.Text);
                _PersonID = ctrlPersonCard1.FillCardWithData(_PersonID);
            }
            else
            {
                _PersonID = ctrlPersonCard1.FillCardWithData(tbFilterValue.Text);
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilterValue.Text = "";
        }

        private void tbFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "PersonID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    errorProvider1.SetError(((TextBox)sender), "Please Insert a Nuber");
                    e.Handled = true;
                }
            }
        }
    }
}
