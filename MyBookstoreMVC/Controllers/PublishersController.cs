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
    public class PublishersController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Publisher record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO publishers VALUES (@pubName)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pubName", record.Name);
                    cmd.ExecuteNonQuery();
                    ViewBag.Success = "<div class='alert alert-success'>Record added.</div>"; // displays alert message when record is successfully added
                    ModelState.Clear(); // removes existing user input
                    return View();
                }
            }
        }

        public ActionResult Index()
        {
            List<Publisher> list = new List<Publisher>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT pubID, pubName FROM publishers";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new Publisher
                            {
                                ID = int.Parse(data["pubID"].ToString()),
                                Name = data["pubName"].ToString()
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
                string query = @"SELECT pubID, pubName
                    FROM publishers
                    WHERE pubID=@pubID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pubID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            Publisher record = new Publisher();
                            while (data.Read())
                            {
                                record.ID = int.Parse(data["pubID"].ToString());
                                record.Name = data["pubName"].ToString();
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
        public ActionResult Details(Publisher record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE publishers SET pubName=@pubName
                    WHERE pubID=@pubID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pubName", record.Name);
                    cmd.Parameters.AddWithValue("@pubID", record.ID);
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
                string query = @"DELETE FROM publishers WHERE pubID=@pubID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pubID", id);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}