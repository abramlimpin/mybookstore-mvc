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
    public class AuthorsController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Author record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO authors VALUES
                    (@authorLN, @authorFN, @authorPhone, @authorAddress,
                    @authorCity, @authorState, @authorZip)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@authorLN", record.LN);
                    cmd.Parameters.AddWithValue("@authorFN", record.FN);
                    cmd.Parameters.AddWithValue("@authorPhone", record.Phone);
                    cmd.Parameters.AddWithValue("@authorAddress", record.Address);
                    cmd.Parameters.AddWithValue("@authorCity", record.City);
                    cmd.Parameters.AddWithValue("@authorState", record.State);
                    cmd.Parameters.AddWithValue("@authorZip", record.Zip);
                    cmd.ExecuteNonQuery();
                    ViewBag.Success = "<div class='alert alert-success'>Record added.</div>"; // displays alert message when record is successfully added
                    ModelState.Clear(); // removes existing user input
                    return View();
                }
            }
        }

        public ActionResult Index()
        {
            List<Author> list = new List<Author>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT authorID, authorLN, authorFN, authorPhone, 
                    authorAddress, authorCity, authorState, authorZip
                    FROM authors";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new Author
                            {
                                ID = int.Parse(data["authorID"].ToString()),
                                LN = data["authorLN"].ToString(),
                                FN = data["authorFN"].ToString(),
                                Phone = data["authorPhone"].ToString(),
                                Address = data["authorAddress"].ToString(),
                                City = data["authorCity"].ToString(),
                                State = data["authorState"].ToString(),
                                Zip = data["authorZip"].ToString()
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
                string query = @"SELECT authorID, authorLN, authorFN, authorPhone,
                    authorAddress, authorCity, authorState, authorZip
                    FROM authors
                    WHERE authorID=@authorID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@authorID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            Author record = new Author();
                            while (data.Read())
                            {
                                record.ID = int.Parse(data["authorID"].ToString());
                                record.LN = data["authorLN"].ToString();
                                record.FN = data["authorFN"].ToString();
                                record.Phone = data["authorPhone"].ToString();
                                record.Address = data["authorAddress"].ToString();
                                record.City = data["authorCity"].ToString();
                                record.State = data["authorState"].ToString();
                                record.Zip = data["authorZip"].ToString();
                            }
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
        public ActionResult Details(Author record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE authors SET
                    authorLN=@authorLN, authorFN=@authorFN, authorPhone=@authorPhone,
                    authorAddress=@authorAddress, authorCity=@authorCity,
                    authorState=@authorState, authorZip=@authorZip
                    WHERE authorID=@authorID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@authorLN", record.LN);
                    cmd.Parameters.AddWithValue("@authorFN", record.FN);
                    cmd.Parameters.AddWithValue("@authorPhone", record.Phone);
                    cmd.Parameters.AddWithValue("@authorAddress", record.Address);
                    cmd.Parameters.AddWithValue("@authorCity", record.City);
                    cmd.Parameters.AddWithValue("@authorState", record.State);
                    cmd.Parameters.AddWithValue("@authorZip", record.Zip);
                    cmd.Parameters.AddWithValue("@authorID", record.ID);
                    cmd.ExecuteNonQuery();
                    ViewBag.Success = "<div class='alert alert-success'>Record updated.</div>"; // displays alert message when record is successfully updated
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
                string query = @"DELETE FROM authors WHERE authorID=@authorID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@authorID", id);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}