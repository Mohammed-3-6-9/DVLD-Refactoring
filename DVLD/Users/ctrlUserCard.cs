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
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID;
        private clsUser _User;
        public int UserID { get => _UserID; }

        private ctrlUserCard()
        {
            InitializeComponent();
        }

        public void FillCardWithData(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.Find(_UserID);

            if (_User == null)
            {
                MessageBox.Show($"Couldn't Find User [ {_UserID} ]");
                _ResetInfo();
                return;
            }

            _FillUserCard();
        }

        private void _ResetInfo()
        {
            _UserID = -1;
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }

        void _FillUserCard()
        {
            ctrlPersonCard1.FillCardWithData(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = (_User.IsActive) ? "Yes" : "No";
        }
    }
}
