using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsManageTestTypeBL
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }

        clsManageTestTypeBL(int ID, string Title, string description, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = description;
            this.Fees = Fees;
        }

        public static clsManageTestTypeBL FindTestTypeByID(int ID)
        {
            string Title = string.Empty;
            decimal Fees = -1;
            string Description = string.Empty;

            if (clsManageTestTypesDAL.GetTestTypeByID(ID, ref Title, ref Description, ref Fees))
            {
                return new clsManageTestTypeBL(ID, Title, Description, Fees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllData()
        {
            return clsManageTestTypesDAL.GetAllData();
        }

        private bool _UpdateApplicationType()
        {
            return clsManageTestTypesDAL.UpdateTestType(this.ID, this.Title, this.Description, this.Fees);
        }

        public bool Save()
        {
            return this._UpdateApplicationType();
        }
    }


}
