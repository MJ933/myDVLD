using System;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsDrivingLicenseServicesDAL
    {
        public static bool GetApplicationByApplicationID(int LDLApplicationID, ref DateTime applicationDate,
                    ref int LicenseClassID, ref decimal applicationFees, ref string createdBy, ref int ApplicantPersonID
            , ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT " +
                    "p.ApplicationID, u.UserName, l.LicenseClassID, p.ApplicationDate, p.PaidFees, p.ApplicantPersonID, p.ApplicationTypeID, p.ApplicationStatus, p.LastStatusDate " +
                    "FROM Applications p " +
                    "INNER JOIN Users u ON u.UserID = p.CreatedByUserID " +
                    "INNER JOIN LocalDrivingLicenseApplications l2 ON p.ApplicationID = l2.ApplicationID " +
                    "INNER JOIN LicenseClasses l ON l2.LicenseClassID = l.LicenseClassID " +
                    "WHERE p.ApplicationID = @ApplicationID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", LDLApplicationID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LDLApplicationID = (int)reader["ApplicationID"];
                            applicationDate = (DateTime)reader["ApplicationDate"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                            applicationFees = (decimal)reader["PaidFees"];
                            createdBy = reader["UserName"].ToString();
                            ApplicantPersonID = (int)reader["ApplicantPersonID"];
                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            ApplicationStatus = (byte)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving application: " + ex.Message);
                }
            }
            return false;
        }

        public static bool GetApplicationByPersonID(int PersonID, ref int LDLApplicationID, ref DateTime applicationDate,
            ref int LicenseClassID, ref decimal applicationFees, ref string createdBy, ref int ApplicantPersonID,
            ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT " +
                    "p.ApplicationID, u.UserName, l.LicenseClassID, p.ApplicationDate, p.PaidFees, p.ApplicantPersonID, p.ApplicationTypeID, p.ApplicationStatus, p.LastStatusDate " +
                    "FROM Applications p " +
                    "INNER JOIN Users u ON u.UserID = p.CreatedByUserID " +
                    "INNER JOIN LocalDrivingLicenseApplications l2 ON p.ApplicationID = l2.ApplicationID " +
                    "INNER JOIN LicenseClasses l ON l2.LicenseClassID = l.LicenseClassID " +
                    "WHERE p.ApplicantPersonID = @ApplicantPersonID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LDLApplicationID = (int)reader["ApplicationID"];
                            applicationDate = (DateTime)reader["ApplicationDate"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                            applicationFees = (decimal)reader["PaidFees"];
                            createdBy = reader["UserName"].ToString();
                            ApplicantPersonID = (int)reader["ApplicantPersonID"];
                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            ApplicationStatus = (byte)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving application: " + ex.Message);
                }
            }
            return false;
        }


        public static bool GetApplicationByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref DateTime applicationDate,
    ref int LicenseClassID, ref decimal applicationFees, ref string createdBy, ref int ApplicantPersonID,
    ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT " +
                    "a.ApplicationID, u.UserName, l.LicenseClassID, a.ApplicationDate, a.PaidFees, a.ApplicantPersonID, a.ApplicationTypeID, a.ApplicationStatus, a.LastStatusDate " +
                    "FROM Applications a " +
                    "INNER JOIN Users u ON u.UserID = a.CreatedByUserID " +
                    "INNER JOIN LocalDrivingLicenseApplications l2 ON a.ApplicationID = l2.ApplicationID " +
                    "INNER JOIN LicenseClasses l ON l2.LicenseClassID = l.LicenseClassID " +
                    "WHERE l2.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ApplicationID = (int)reader["ApplicationID"];
                            applicationDate = (DateTime)reader["ApplicationDate"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                            applicationFees = (decimal)reader["PaidFees"];
                            createdBy = reader["UserName"].ToString();
                            ApplicantPersonID = (int)reader["ApplicantPersonID"];
                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            ApplicationStatus = (byte)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving application: " + ex.Message);
                }
            }
            return false;
        }


        public static bool NotHaveActiveApplicationOfSameLicenseClass(int LicenseClassID, int ApplicantPersonID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                SELECT * 
                FROM LocalDrivingLicenseApplications l 
                INNER JOIN Applications a 
                ON a.ApplicationID = l.ApplicationID 
                WHERE l.LicenseClassID = @LicenseClassID 
                AND a.ApplicantPersonID = @ApplicantPersonID
                AND a.ApplicationStatus =1
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // If a record is found, return false indicating there is an active application
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error retrieving application: " + ex.Message);
                    // Rethrow the exception or handle it as per your application's error handling strategy
                    //throw;
                }
            }

            // If no record is found, return true indicating there is no active application
            return true;
        }

        public static int AddNewLocalApplication(string CreatedBy, DateTime ApplicationDate,
            decimal PaidFees, int LicenseClassID, int ApplicantPersonID, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID,
                @ApplicationStatus, @LastStatusDate, @PaidFees, (SELECT UserID FROM Users WHERE UserName = @CreatedBy));
                SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        // Insert into LocalDrivingLicenseApplications table
                        string licenseQuery = @"
                        INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                        VALUES (@ApplicationID, @LicenseClassID);";

                        SqlCommand licenseCommand = new SqlCommand(licenseQuery, connection);
                        licenseCommand.Parameters.AddWithValue("@ApplicationID", InsertedID);
                        licenseCommand.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        connection.Open();
                        licenseCommand.ExecuteNonQuery();
                        connection.Close();

                        return InsertedID;
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error adding new application: " + ex.Message);
                }
            }
            return -1;
        }

        public static bool UpdateLocalApplication(int ApplicationID, string CreatedBy,
    DateTime ApplicationDate, decimal PaidFees, int LicenseClassID, int ApplicantPersonID, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate)
        {
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
        UPDATE Applications SET
            CreatedByUserID = (SELECT UserID FROM Users WHERE UserName = @CreatedBy),
            ApplicationDate = @ApplicationDate,
            PaidFees = @PaidFees,
            ApplicantPersonID = @ApplicantPersonID,
            ApplicationTypeID = @ApplicationTypeID,
            ApplicationStatus = @ApplicationStatus,
            LastStatusDate = @LastStatusDate
        WHERE ApplicationID = @ApplicationID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);

                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();

                    // Update LocalDrivingLicenseApplications table
                    string licenseQuery = @"
            UPDATE LocalDrivingLicenseApplications SET
                LicenseClassID = @LicenseClassID
            WHERE ApplicationID = @ApplicationID;";

                    SqlCommand licenseCommand = new SqlCommand(licenseQuery, connection);
                    licenseCommand.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    licenseCommand.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    RowsAffected += licenseCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error updating application: " + ex.Message);
                    return false;
                }
            }
            return RowsAffected > 0;
        }

        public static bool CancelLocalApplicationStatus(int ApplicationID, byte ApplicationStatus, DateTime LastStatusDate)
        {
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
            UPDATE Applications SET
                ApplicationStatus = @ApplicationStatus,
                LastStatusDate = @LastStatusDate
            WHERE ApplicationID = @ApplicationID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);

                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error updating application: " + ex.Message);
                    return false;
                }
            }
            return RowsAffected > 0;
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error deleting application: " + ex.Message);
                }
            }
            return RowsAffected > 0;
        }
    }
}