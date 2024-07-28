using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Security;
using WebApp26.Models;


namespace WebApp26.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(UserCredentials user)
        {
            Session["Username"] = user.Username;
            if (ModelState.IsValid)
            {
                // Insert user data into the database
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO UserCredentials (Username, CreatedDate, Gender, Password, Address ) VALUES (@Username, @CreatedDate, @Gender, @Password, @Address)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
                    command.Parameters.AddWithValue("@Gender", user.Gender);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Address", user.Address);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Success", "Account");
            }

            return View(user);
        }
        // GET: /Account/Success
        public ActionResult Success()
        {
            return View();
        }

        public ActionResult DisplayData()
        {
            List<UserCredentials> users = new List<UserCredentials>();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM UserCredentials";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserCredentials user = new UserCredentials
                    {
                        Username = reader["Username"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        Gender = reader["Gender"].ToString(),
                        Password = reader["Password"].ToString(),
                        Address = reader["Address"].ToString()
                    };
                    users.Add(user);
                }
            }

            return View(users);
        }

        public ActionResult LatestCandidate()
        {
            // Retrieve the data for the latest candidate
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 * FROM UserCredentials ORDER BY CreatedDate DESC";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    UserCredentials latestCandidate = new UserCredentials
                    {
                        Username = reader["Username"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        Gender = reader["Gender"].ToString(),
                        Password = reader["Password"].ToString(),
                        Address = reader["Address"].ToString()
                    };

                    return View(latestCandidate);
                }
            }

            // Return an empty view if no data found
            return View(new UserCredentials());
        }
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string newPassword)
        {
            // For demonstration purposes, assuming a user is logged in and their username is known
            string username = Session["Username"] as string;

            // Update password in the database
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE UserCredentials SET Password = @NewPassword WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("PasswordUpdate");
        }

        public ActionResult PasswordUpdate()
        {
            return View();
        }



    }
}
//I have modified.