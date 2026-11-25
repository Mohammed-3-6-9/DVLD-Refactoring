using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class clsTestType
    {
        public clsGeneral.enTestTypes TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public decimal TestTypeFees { get; set; }

        public clsTestType()
        {
            TestTypeID = clsGeneral.enTestTypes.Vision;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0;
        }

        private clsTestType(clsGeneral.enTestTypes TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
        }

        private bool _Update()
        {
            return clsTestTypesData.UpdateTestType((int)this.TestTypeID,this.TestTypeTitle,
                this.TestTypeDescription, this.TestTypeFees);
        }

        public static clsTestType Find(clsGeneral.enTestTypes TestTypeID)
        {
            string TestTypeTitle = "", TestTypeDescription = ";";
            decimal TestFees = 0;

            if (clsTestTypesData.GetTestTypeInfo((int)TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestFees))
            {
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestFees);
            }
            else
                return null;
        }

        public bool Save()
        {
            return _Update();
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }
    }
}