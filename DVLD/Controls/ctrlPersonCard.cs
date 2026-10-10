using Business_Logic;
using DVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        private clsPerson _Person = new clsPerson();
        public byte PersonAge
        {
            get
            {
                if (_Person == null) return 0;
                byte age = (byte)(DateTime.Now.Year - _Person.DateOfBirth.Year);
                if (_Person.DateOfBirth.Date > DateTime.Now.AddYears(-age)) age--;
                return age;
            }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            pbPersonImage.Image = (_Person.Gendor == 0) ? Properties.Resources.Male_512 : Properties.Resources.Female_512;

            if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
                pbPersonImage.ImageLocation = _Person.ImagePath;
            else
                MessageBox.Show("Could not find this image: = " + _Person.ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _RefreshPersonCard()
        {
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _PersonID.ToString();
            lblName.Text = _Person.FullName;
            lblGendor.Text = (_Person.Gendor == 0) ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString();
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountry.Find(_Person.NationalCountryID).CountryName;
            _LoadPersonImage();
        }

        public void ResetDefaultValues()
        {
            _PersonID = -1;
            _Person = null;
            lblPersonID.Text = "[???]";
            lblName.Text = "[???]";
            lblNationalNo.Text = "[???]";
            lblGendor.Text = "[???]";
            lblEmail.Text = "[???]";
            lblAddress.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            lblPhone.Text = "[???]";
            lblCountry.Text = "[???]";
            pbPersonImage.Image = Properties.Resources.Male_512;
        }

        private void lblEditPersonLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(_PersonID);
            frm.ShowDialog();
            FillCardWithData(_PersonID);
        }

        public int FillCardWithData(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                ResetDefaultValues();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return -1;
            }

            _RefreshPersonCard();
            return _Person.PersonID;
        }

        public int FillCardWithData(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
            {
                ResetDefaultValues();
                return -1;
            }

            _Person = clsPerson.Find(NationalNumber.Trim());
            if (_Person == null)
            {
                ResetDefaultValues();
                MessageBox.Show("No Person with National No. = " + NationalNumber.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            _RefreshPersonCard();
            return _Person.PersonID;
        }
    }
}