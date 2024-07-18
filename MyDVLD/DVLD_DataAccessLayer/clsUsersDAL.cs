using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsUsersDAL
    {
        private clsPeopleDAL _peopleDAL;

        public clsUsersDAL()
        {
            _peopleDAL = new clsPeopleDAL();
        }

        public bool GetPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email,
            ref int NationalityCountryID, ref string ImagePath)
        {
            return clsPeopleDAL.GetPersonByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);
        }

        public bool CheckPersonNationalNo(string NationalNo)
        {
            return clsPeopleDAL.CheckPersonNationalNo(NationalNo);
        }

        public static DataTable GetAllData()
        {
            DataTable dataTable1 = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                                select UserID as 'User ID',people.PersonID as 'Person ID', People.FirstName +' '+
                                People.SecondName +' '+ People.ThirdName +' '+ People.LastName as 'Full Name',
                                UserName as 'User Name',IsActive as 'Is Active' from users 
                                inner join People on Users.PersonID = People.PersonID ;";

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

        public int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName,
              string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
              int NationalityCountryID, string ImagePath)
        {
            return clsPeopleDAL.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        public bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
            string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            return clsPeopleDAL.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        public bool DeletePerson(int ID)
        {
            return clsPeopleDAL.DeletePerson(ID);
        }


        public static bool GetUserByUserID(int userID, ref int personID, ref string userName, ref string password, ref bool isActive)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personID = (int)reader["PersonID"];
                            userName = reader["UserName"].ToString();
                            password = reader["Password"].ToString();
                            isActive = (bool)reader["IsActive"];
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

        public static bool GetUserByPersonID(ref int userID, int PersonID, ref string userName, ref string password, ref bool isActive)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Users WHERE PersonID = @PersonID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userID = (int)reader["UserID"];
                            userName = reader["UserName"].ToString();
                            password = reader["Password"].ToString();
                            isActive = (bool)reader["IsActive"];
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


        //public int AddNewUser(string NationalNo, string FirstName, string SecondName, string ThirdName,
        //              string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
        //              int NationalityCountryID, string ImagePath, string UserName, string Password, bool IsActive)
        //{
        //    int PersonID = AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        //    if (PersonID != -1)
        //    {
        //        return AddNewUser(PersonID, UserName, Password, IsActive);
        //    }
        //    return -1;
        //}

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                INSERT INTO Users (PersonID, UserName, Password, IsActive)
                VALUES (@PersonID, @UserName, @Password, @IsActive);
                SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        return InsertedID;
                    }
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error adding new user: " + ex.Message);
                }
            }
            return -1;
        }

        //public bool UpdateUser(int UserID, int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
        //    string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
        //    int NationalityCountryID, string ImagePath, string UserName, string Password, bool IsActive)
        //{
        //    bool PersonUpdated = UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        //    if (PersonUpdated)
        //    {
        //        return UpdateUser(UserID, PersonID, UserName, Password, IsActive);
        //    }
        //    return false;
        //}

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                UPDATE Users SET
                    PersonID = @PersonID,
                    UserName = @UserName,
                    Password = @Password,
                    IsActive = @IsActive
                WHERE UserID = @UserID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive);

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


        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", UserID);
                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception (optional)
                    Console.WriteLine("Error deleting user: " + ex.Message);
                }
            }
            return RowsAffected > 0;
        }
    }
}