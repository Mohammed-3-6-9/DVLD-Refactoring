using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode = enMode.AddNew;
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo;

        private string _UserName;
        public string UserName
        {
            get => _UserName;
            set => _UserName = value;
        }
        private string _PrevUserName { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }

        public clsUser()
        {
            PersonID = -1;
            UserID = -1;
            IsActive = false;
            _UserName = "";
            _PrevUserName = "";
            Password = "";
            _Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string UserName,
            string Password, bool IsActive)
        {

            this.PersonID =PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserID = UserID;
            this.Password = DecryptPassword(Password);
            this.IsActive = IsActive;
            this.UserName = UserName;
            this._PrevUserName = this.UserName;
            _Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.UserID = clsUsersData.AddNewUser(PersonID,UserName, EncryptPassword(this.Password), IsActive);

            return (this.UserID != -1);
        }

        private bool _Update()
        {
            return clsUsersData.UpdateUser(this.UserID, this.PersonID, this.UserName,
                EncryptPassword(this.Password), this.IsActive);
        }

        public static clsUser Find(int ID)
        {
            int PersonID = -1;
            string Password = "", UserName = "";
            bool IsActive = false;

            if (clsUsersData.GetUserInfoByID(ID, ref PersonID, ref UserName , ref Password,
                ref IsActive))
            {
                return new clsUser(ID, PersonID, UserName , Password, IsActive);
            }
            else
                return null;
        }

        public static clsUser FindbyUserNameAndPassword(string UserName, string Password)
        {
            int PersonID = -1, UserID = -1;
            bool IsActive = false;
            Password = EncryptPassword(Password);

            if (clsUsersData.FindUserByUserNameAndPassword(ref UserID, ref PersonID, ref UserName, ref Password,
                ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }

        private bool _ValidateProberties()
        {
            if (string.IsNullOrWhiteSpace(this.Password) ||
               string.IsNullOrWhiteSpace(this.UserName))
            {
                return false;
            }

            if (_UserName != _PrevUserName && clsUser.IsUserNameExist(_UserName))
            {
                return false;
            }

            return true;
        }

        public bool Save()
        {
            if (!_ValidateProberties())
                return false;

            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNew())
                        {
                            _Mode = enMode.Update;
                            _PrevUserName = _UserName;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case enMode.Update:
                    {
                        if (_Update())
                        {
                            _PrevUserName = _UserName;
                            return true;
                        }
                        else
                            return false;
                    }
            }

            return false;
        }

        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUsersData.IsUserExist(UserID);
        }

        private bool _ChangePassword()
        {
            return clsUsersData.ChangePassword(this.UserID, EncryptPassword(this.Password));
        }


        public static bool ChangePassword(int UserID,string OldPassword,string NewPassword)
        {
            if (string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(OldPassword))
            {
                return false;
            }

            clsUser user = clsUser.Find(UserID);

            if (user == null)
                return false;

            if(user.Password == OldPassword)
            {
                user.Password = NewPassword;
                return user._ChangePassword();
            }

            return false;
        }

        public static bool IsUserNameExist(string UserName)
        {
            if (clsUsersData.IsUserNameExist(UserName))
            {
                return true;
            }

            return false;
        }

        public static string DecryptPassword(string encyrpitedPassword)
        {
            if (string.IsNullOrWhiteSpace(encyrpitedPassword))
                return "";

            StringBuilder Password = new StringBuilder();

            for (int i = 0; i < encyrpitedPassword.Length; i++)
            {
                Password.Append((char)((int)encyrpitedPassword[i] - 5));
            }

             return Password.ToString();
        }

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "";

            StringBuilder EncyrpitedPassword = new StringBuilder();

            for (int i = 0; i < password.Length; i++)
            {
                EncyrpitedPassword.Append((char)((int)password[i] + 5));
            }

            return EncyrpitedPassword.ToString();
        }

        public static bool IsUserExistByNationalNumber(string NationalNumber)
        {
            return clsUsersData.IsUserExistByNationalNumber(NationalNumber);
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            return clsUsersData.IsUserExistByPersonID(PersonID);
        }
    }
}
