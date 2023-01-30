using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ASPSession.Models;
using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using CA_ShoppingCart.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPSession.Controllers
{
    public class RatingController : Controller
    {

        private IConfiguration _configuration;

        public RatingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index(int productId)
        {
            ViewData["productId"] = productId;
            return View();
        }

        public string GetStar(int pid)
        {
            Star star = null;

            string connStr = _configuration.GetConnectionString("db_conn");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string q = String.Format("SELECT * FROM Rating where CustomerId = 11 and ProductId = {0}", pid);

                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            star = new Star
                            {
                                UserId = reader.GetInt32(0),
                                ProductId = reader.GetInt32(1),
                                Rating = reader.GetInt32(2)
                            };
                        }
                    }
                }

                conn.Close();
            }


            return JsonSerializer.Serialize(star);

        }

        public string SetStarRating(int UserId, int ProductId, int Rating)
        {

            bool status = SetStar(UserId, ProductId, Rating);

            return status ? "success" : "fail";
        }

        public bool SetStar(int cid, int pid, int rid)
        {
            if (UpdateStar(cid, pid, rid))
            {
                return true;
            }

            return AddStar(cid, pid, rid);
        }

        public bool UpdateStar(int cid, int pid, int rid)
        {
            bool status = false;
            string connStr = _configuration.GetConnectionString("db_conn");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string q = String.Format(@"update Rating set ratingValue = {0} where CustomerId = {1} and ProductId = {2}", rid, cid, pid);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        status = true;
                    }
                }
                conn.Close();
            }
            return status;
        }

        public bool AddStar(int cid, int pid, int rid)
        {
            bool status = false;
            string connStr = _configuration.GetConnectionString("db_conn");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string q = String.Format("insert into Rating values ({0},{1}, {2})", cid, pid, rid);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        status = true;
                    }
                }
                conn.Close();
            }
            return status;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

