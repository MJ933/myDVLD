using DVLD_DataAccessLayer;
using System;

namespace DVLD_BusinessLayer
{
    public class clsDrivingLicenseServicesBL
    {
        public enum enMode
        { AddNew = 1, Update = 2 };
        public enMode Mode { get; set; } = enMode.AddNew;

        public int LDLApplicationID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int LicenseClassID { get; set; }
        public decimal ApplicationFees { get; set; }
        public string CreatedBy { get; set; }
        public int PersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }

        public clsDrivingLicenseServicesBL()
        {
            this.LDLApplicationID = -1;
            this.ApplicationDate = DateTime.MinValue;
            this.LicenseClassID = 0;
            this.ApplicationFees = 0;
            this.CreatedBy = string.Empty;
            this.PersonID = 0;
            this.ApplicationTypeID = 0;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.MinValue;
        }

        public clsDrivingLicenseServicesBL(int LDLApplicationID, DateTime applicationDate, int licenseClassID, decimal applicationFees,
            string createdBy, int personID, int applicationTypeID, byte applicationStatus, DateTime lastStatusDate)
        {
            this.LDLApplicationID = LDLApplicationID;
            this.ApplicationDate = applicationDate;
            this.LicenseClassID = licenseClassID;
            this.ApplicationFees = applicationFees;
            this.CreatedBy = createdBy;
            this.PersonID = personID;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.Mode = enMode.Update;
        }

        //public static clsDrivingLicenseServicesBL FindApplicationByApplicationID(int ApplicationID)
        //{
        //    DateTime ApplicationDate = DateTime.MinValue;
        //    int LicenseClassID = 0;
        //    decimal ApplicationFees = 0;
        //    string CreatedBy = string.Empty;

        //    if (clsDrivingLicenseServicesDAL.GetApplicationByApplicationID(ApplicationID, ref ApplicationDate, ref LicenseClassID, ref ApplicationFees, ref CreatedBy))
        //        return new clsDrivingLicenseServicesBL(ApplicationID, ApplicationDate, LicenseClassID, ApplicationFees, CreatedBy, 0, 0, 0, DateTime.Today);
        //    else return null;
        //}

        //public static clsDrivingLicenseServicesBL FindApplicationByPersonID(int LDLApplicationID)
        //{
        //    int ApplicationID = -1;
        //    DateTime ApplicationDate = DateTime.MinValue;
        //    int LicenseClassID = 0;
        //    decimal ApplicationFees = 0;
        //    string CreatedBy = string.Empty;

        //    if (clsDrivingLicenseServicesDAL.GetApplicationByPersonID(LDLApplicationID, ref ApplicationID, ref ApplicationDate, ref LicenseClassID, ref ApplicationFees, ref CreatedBy))
        //        return new clsDrivingLicenseServicesBL(ApplicationID, ApplicationDate, LicenseClassID, ApplicationFees, CreatedBy, LDLApplicationID, 0, 0, DateTime.MinValue);
        //    else return null;
        //}
        public static clsDrivingLicenseServicesBL FindApplicationByApplicationID(int LDLApplicationID)
        {
            DateTime ApplicationDate = DateTime.MinValue;
            int LicenseClassID = 0;
            decimal ApplicationFees = 0;
            string CreatedBy = string.Empty;
            int ApplicantPersonID = 0;
            int ApplicationTypeID = 0;
            byte ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.MinValue;

            if (clsDrivingLicenseServicesDAL.GetApplicationByApplicationID(LDLApplicationID, ref ApplicationDate,
                ref LicenseClassID, ref ApplicationFees, ref CreatedBy, ref ApplicantPersonID, ref ApplicationTypeID,
                ref ApplicationStatus, ref LastStatusDate))
                return new clsDrivingLicenseServicesBL(LDLApplicationID, ApplicationDate, LicenseClassID,
                    ApplicationFees, CreatedBy, ApplicantPersonID, ApplicationTypeID, ApplicationStatus, LastStatusDate);
            else return null;
        }

        public static clsDrivingLicenseServicesBL FindApplicationByPersonID(int PersonID)
        {
            int LDLApplicationID = -1;
            DateTime ApplicationDate = DateTime.MinValue;
            int LicenseClassID = 0;
            decimal ApplicationFees = 0;
            string CreatedBy = string.Empty;
            int ApplicantPersonID = 0;
            int ApplicationTypeID = 0;
            byte ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.MinValue;

            if (clsDrivingLicenseServicesDAL.GetApplicationByPersonID(PersonID, ref LDLApplicationID, ref ApplicationDate,
                ref LicenseClassID, ref ApplicationFees, ref CreatedBy, ref ApplicantPersonID, ref ApplicationTypeID,
                ref ApplicationStatus, ref LastStatusDate))
                return new clsDrivingLicenseServicesBL(LDLApplicationID, ApplicationDate, LicenseClassID, ApplicationFees,
                    CreatedBy, ApplicantPersonID, ApplicationTypeID, ApplicationStatus, LastStatusDate);
            else return null;
        }




        public static clsDrivingLicenseServicesBL FindApplicationByLDLApplicationID(int LDLApplicationID)
        {
            int ApplicationID = -1;
            DateTime ApplicationDate = DateTime.MinValue;
            int LicenseClassID = 0;
            decimal ApplicationFees = 0;
            string CreatedBy = string.Empty;
            int ApplicantPersonID = 0;
            int ApplicationTypeID = 0;
            byte ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.MinValue;

            if (clsDrivingLicenseServicesDAL.GetApplicationByLocalDrivingLicenseApplicationID(LDLApplicationID, ref ApplicationID, ref ApplicationDate,
                ref LicenseClassID, ref ApplicationFees, ref CreatedBy, ref ApplicantPersonID, ref ApplicationTypeID,
                ref ApplicationStatus, ref LastStatusDate))
                return new clsDrivingLicenseServicesBL(ApplicationID, ApplicationDate, LicenseClassID, ApplicationFees,
                    CreatedBy, ApplicantPersonID, ApplicationTypeID, ApplicationStatus, LastStatusDate);
            else return null;
        }



        private bool _AddNewLocalApplication()
        {
            this.LDLApplicationID = clsDrivingLicenseServicesDAL.AddNewLocalApplication(this.CreatedBy, this.ApplicationDate,
                this.ApplicationFees, this.LicenseClassID, this.PersonID, this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate);
            return this.LDLApplicationID != -1;
        }

        private bool _UpdateLocalApplication()
        {
            return clsDrivingLicenseServicesDAL.UpdateLocalApplication(this.LDLApplicationID, this.CreatedBy, this.ApplicationDate,
                this.ApplicationFees, this.LicenseClassID, this.PersonID, this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate);
        }

        public bool CancelLocalApplicationStatus()
        {
            return clsDrivingLicenseServicesDAL.CancelLocalApplicationStatus(this.LDLApplicationID, 2, DateTime.Today);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (this._AddNewLocalApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return this._UpdateLocalApplication();
                default:
                    return false;
            }
        }

        public static bool NotHaveActiveApplicationOfSameLicenseClass(int LicenseClassID, int ApplicantPersonID)
        {
            return clsDrivingLicenseServicesDAL.NotHaveActiveApplicationOfSameLicenseClass(LicenseClassID, ApplicantPersonID);
        }
    }

}