using MyBookstoreMVC.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookstoreMVC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns total number of records based of selected database table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        int CountRecords(string table)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT COUNT(*) FROM " + table;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public ActionResult Index()
        {
            ViewBag.TotalAuthors = CountRecords("authors");
            ViewBag.TotalPublishers = CountRecords("publishers");
            ViewBag.TotalTitles = CountRecords("titles");
            ViewBag.TotalUsers = CountRecords("users");
            return View();
        }
    }
}