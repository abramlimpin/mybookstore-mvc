using MyBookstoreMVC.App_Code;
using MyBookstoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookstoreMVC.Controllers
{
    public class UsersController : Controller
    {
        /// <summary>
        /// Displays list of publishers from Publishers table
        /// </summary>
        /// <returns></returns>
        public List<UserType> GetUserTypes()
        {
            List<UserType> list = new List<UserType>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT typeID, typeName
                    FROM types
                    ORDER BY typeName";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new UserType
                            {
                                ID = int.Parse(data["typeID"].ToString()),
                                Name = data["typeName"].ToString()
                            });
                        }
                        return list;
                    }
                }
            }
        }

        /// <summary>
        /// Displays list of status
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Active", Value = "Active" });
            list.Add(new SelectListItem() { Text = "Inactive", Value = "Inactive" });
            list.Add(new SelectListItem() { Text = "Blocked", Value = "Blocked" });
            list.Add(new SelectListItem() { Text = "Archived", Value = "Archived" });
            return list;
        }

        public ActionResult Add()
        {
            User record = new User();
            record.Types = GetUserTypes();
            record.Status = GetStatus();
            return View(record);
        }
        /// <summary>
        /// Checks if email address is existing from users table
        /// </summary>
        /// <param name="email">User input</param>
        /// <returns></returns>
        public static bool IsExisting(string email)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT userEmail FROM users WHERE userEmail=@userEmail";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userEmail", email);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        [HttpPost]
        public ActionResult Add(User record)
        {
            if (IsExisting(record.Email))
            {
                ViewBag.Message = "<div class='alert alert-danger'>Email address already existing.</div>"; // displays error message
                record.Types = GetUserTypes();
                return View(record);
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
                {
                    con.Open();
                    string query = @"INSERT INTO users VALUES
                    (@typeID, @userEmail, @userPW, @userFN,
                    @userLN, @userPhone, @userAddress, @userStatus)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@typeID", record.TypeID);
                        cmd.Parameters.AddWithValue("@userEmail", record.Email);
                        cmd.Parameters.AddWithValue("@userPW", Helper.Hash(record.Password));
                        cmd.Parameters.AddWithValue("@userFN", record.FN);
                        cmd.Parameters.AddWithValue("@userLN", record.LN);
                        cmd.Parameters.AddWithValue("@userPhone", record.Phone);
                        cmd.Parameters.AddWithValue("@userAddress", record.Address);
                        cmd.Parameters.AddWithValue("@userStatus", "Active");
                        cmd.ExecuteNonQuery();
                        ViewBag.Message = "<div class='alert alert-success'>Record added.</div>"; // displays alert message when record is successfully added
                        ModelState.Clear(); // removes existing user input

                        record.Types = GetUserTypes();
                        return View(record);
                    }
                }
            }
        }

        public ActionResult Index()
        {
            List<User> list = new List<User>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT u.userID, t.typeName, u.userEmail,
                    u.userFN, u.userLN, u.userPhone, u.userAddress,
                    u.userStatus
                    FROM users u
                    INNER JOIN types t ON u.typeID = t.typeID
                    WHERE u.userStatus!=@userStatus";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userStatus", "Archived");
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new User
                            {
                                ID = int.Parse(data["userID"].ToString()),
                                UserType = data["typeName"].ToString(),
                                Email = data["userEmail"].ToString(),
                                FN = data["userFN"].ToString(),
                                LN = data["userLN"].ToString(),
                                Phone = data["userPhone"].ToString(),
                                Address = data["userAddress"].ToString(),
                                CurrentStatus = data["userStatus"].ToString()
                            });
                        }
                    }
                }
            }
            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT userID, typeID, userEmail,
                    userFN, userLN, userPhone, userAddress, userStatus
                    FROM users
                    WHERE userID=@userID AND userStatus!=@userStatus";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userID", id);
                    cmd.Parameters.AddWithValue("@userStatus", "Archived");
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            User record = new User();
                            while (data.Read())
                            {
                                record.ID = int.Parse(data["userID"].ToString());
                                record.TypeID = int.Parse(data["typeID"].ToString());
                                record.Email = data["userEmail"].ToString();
                                record.FN = data["userFN"].ToString();
                                record.LN = data["userLN"].ToString();
                                record.Phone = data["userPhone"].ToString();
                                record.Address = data["userAddress"].ToString();
                                record.CurrentStatus = data["userStatus"].ToString();
                            }
                            record.Types = GetUserTypes();
                            record.Status = GetStatus();
                            return View(record);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Details(User record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = "";
                if (record.Password == string.Empty)
                {
                    query = @"UPDATE users SET typeID=@typeID, userEmail=@userEmail,
                        userFN=@userFN, userLN=@userLN, userPhone=@userPhone,
                        userAddress=@userAddress, userStatus=@userStatus
                        WHERE userID=@userID";
                }
                else
                {
                    query = @"UPDATE users SET typeID=@typeID, userEmail=@userEmail,
                        userPW=@userPW,
                        userFN=@userFN, userLN=@userLN, userPhone=@userPhone,
                        userAddress=@userAddress, userStatus=@userStatus
                        WHERE userID=@userID";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@typeID", record.TypeID);
                    cmd.Parameters.AddWithValue("@userEmail", record.Email);
                    cmd.Parameters.AddWithValue("@userPW", Helper.Hash(record.Password));
                    cmd.Parameters.AddWithValue("@userFN", record.FN);
                    cmd.Parameters.AddWithValue("@userLN", record.LN);
                    cmd.Parameters.AddWithValue("@userPhone", record.Phone);
                    cmd.Parameters.AddWithValue("@userAddress", record.Address);
                    cmd.Parameters.AddWithValue("@userStatus", record.CurrentStatus);
                    cmd.Parameters.AddWithValue("@userID", record.ID);
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "<div class='alert alert-success'>Record updated.</div>"; // displays alert message when record is successfully updated
                    record.Types = GetUserTypes();
                    record.Status = GetStatus();
                    return View(record);
                }
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE users SET userStatus=@userStatus WHERE userID=@userID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userStatus", "Archived");
                    cmd.Parameters.AddWithValue("@userID", id);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}