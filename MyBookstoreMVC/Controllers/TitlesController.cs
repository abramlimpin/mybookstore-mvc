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
    public class TitlesController : Controller
    {
        /// <summary>
        /// Displays list of publishers from Publishers table
        /// </summary>
        /// <returns></returns>
        public List<Publisher> GetPublishers()
        {
            List<Publisher> list = new List<Publisher>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT pubID, pubName
                    FROM publishers
                    ORDER BY pubName";
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
                        return list;
                    }
                }
            }
        }

        public List<Author> GetAuthors()
        {
            List<Author> list = new List<Author>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT authorID, authorLN + ', ' + authorFN AS authorName
                    FROM authors
                    ORDER BY authorLN";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new Author
                            {
                                ID = int.Parse(data["authorID"].ToString()),
                                FullName = data["authorName"].ToString()
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public ActionResult Add()
        {
            Title record = new Title();
            record.Publishers = GetPublishers();
            record.Authors = GetAuthors();
            return View(record);
        }

        [HttpPost]
        public ActionResult Add(Title record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO titles VALUES
                    (@pubID, @authorID, @titleName, @titlePrice,
                    @titlePubDate, @titleNotes)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pubID", record.PubID);
                    cmd.Parameters.AddWithValue("@authorID", record.AuthorID);
                    cmd.Parameters.AddWithValue("@titleName", record.Name);
                    cmd.Parameters.AddWithValue("@titlePrice", record.Price);
                    cmd.Parameters.AddWithValue("@titlePubDate", record.PubDate);
                    cmd.Parameters.AddWithValue("@titleNotes", record.Notes);
                    cmd.ExecuteNonQuery();
                    ViewBag.Success = "<div class='alert alert-success'>Record added.</div>"; // displays alert message when record is successfully added
                    ModelState.Clear(); // removes existing user input

                    record.Publishers = GetPublishers();
                    record.Authors = GetAuthors();
                    return View(record);
                }
            }
        }

        public ActionResult Index()
        {
            List<Title> list = new List<Title>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT t.titleID, p.pubName, a.authorLN + ', ' + authorFN AS authorName,
                    t.titleName, t.titlePrice, t.titlePubDate, t.titleNotes
                    FROM titles t
                    INNER JOIN publishers p ON t.pubID = p.pubID
                    INNER JOIN authors a ON t.authorID = a.authorID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new Title
                            {
                                ID = int.Parse(data["titleID"].ToString()),
                                Publisher = data["pubName"].ToString(),
                                Author = data["authorName"].ToString(),
                                Name = data["titleName"].ToString(),
                                Price = decimal.Parse(data["titlePrice"].ToString()),
                                PubDate = DateTime.Parse(data["titlePubDate"].ToString()),
                                Notes = data["titleNotes"].ToString()
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
                string query = @"SELECT titleID, pubID, authorID,
                    titleName, titlePrice, titlePubDate, titleNotes
                    FROM titles
                    WHERE titleID=@titleID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@titleID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            Title record = new Title();
                            while (data.Read())
                            {
                                record.ID = int.Parse(data["titleID"].ToString());
                                record.PubID = int.Parse(data["pubID"].ToString());
                                record.AuthorID = int.Parse(data["authorID"].ToString());
                                record.Name = data["titleName"].ToString();
                                record.Price = decimal.Parse(data["titlePrice"].ToString());
                                record.PubDate = DateTime.Parse(data["titlePubDate"].ToString());
                                record.Notes = data["titleNotes"].ToString();
                            }
                            record.Publishers = GetPublishers();
                            record.Authors = GetAuthors();
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
                string query = @"DELETE FROM titles WHERE titleID=@titleID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@titleID", id);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}