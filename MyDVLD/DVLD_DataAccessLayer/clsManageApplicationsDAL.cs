using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsManageApplicationsDAL
    {
        public static bool GetTestTypeByID(int ID, ref string Title, ref string Description, ref decimal Fees)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM TestTypes where TestTypeID = @TestTypeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("TestTypeID", ID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Title = reader["TestTypeTitle"].ToString();
                            Description = reader["TestTypeDescription"].ToString();
                            Fees = reader.GetDecimal(reader.GetOrdinal("TestTypeFees"));
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving user: " + ex.Message);
                }
            }
            return false;
        }

        public static bool UpdateTestType(int ID, string Title, string Description, decimal Fees)
        {
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                UPDATE TestTypes SET
                    TestTypeTitle = @TestTypeTitle,
                     TestTypeDescription = @TestTypeDescription,
                    TestTypeFees = @TestTypeFees
                WHERE TestTypeID = @TestTypeID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestTypeID", ID);
                command.Parameters.AddWithValue("@TestTypeTitle", Title);
                command.Parameters.AddWithValue("@TestTypeDescription", Description);
                command.Parameters.AddWithValue("@TestTypeFees", Fees);

                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error updating user: " + ex.Message);
                    return false;
                }
            }
            return RowsAffected > 0;
        }

        public static DataTable GetAllData()
        {
            DataTable dataTable1 = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT
    L.LocalDrivingLicenseApplicationID AS 'L.D.L.AppID',
    Lc.ClassName AS 'Driving Class',
    p.NationalNo AS 'National No',
    p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName AS 'Full Name',
    a.ApplicationDate AS 'Application Date',
    (SELECT COUNT(ta.TestTypeID)
     FROM TestAppointments ta
     INNER JOIN LocalDrivingLicenseApplications L2
     ON L2.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
     INNER JOIN Tests t
     ON t.TestAppointmentID = ta.TestAppointmentID
     WHERE L2.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
     AND t.TestResult = 1) AS 'Passed Test',
    CASE
        WHEN a.ApplicationStatus = 1 THEN 'New'
        WHEN a.ApplicationStatus = 2 THEN 'Canceled'
        WHEN a.ApplicationStatus = 3 THEN 'Completed'
    END AS 'Status'
FROM
    LocalDrivingLicenseApplications L
INNER JOIN
    LicenseClasses Lc
    ON Lc.LicenseClassID = L.LicenseClassID
INNER JOIN
    Applications a
    ON a.ApplicationID = L.ApplicationID
INNER JOIN
    People p
    ON p.PersonID = a.ApplicantPersonID;";

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable1.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving data: " + ex.Message);
                }
            }
            return dataTable1;
        }

        public static DataTable GetAllApplicationTypesData()
        {
            DataTable dataTable1 = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * from ApplicationTypes;";

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable1.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving data: " + ex.Message);
                }
            }
            return dataTable1;
        }


    }
}