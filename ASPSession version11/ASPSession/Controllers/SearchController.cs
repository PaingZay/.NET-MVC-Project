using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPSession.Connectors;
using ASPSession.Data;
using ASPSession.Models;
using ASPSession.Security;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPSession.Controllers
{
    public class SearchController : Controller
    {

        private readonly ISecurity sec;
        private readonly IConnector conn;

        public SearchController(ISecurity sec, IConnector conn)
        {
            this.sec = sec;
            this.conn = conn;

        }
        public IActionResult Index(string searchStr)
        {

            //for rating
            float totalProducts = conn.GetTotalProducts();
            Dictionary<int, Rating> allRatingsList = new Dictionary<int, Rating>();
            for (int i = 1; i <= totalProducts; i++)
            {
                Rating rating = conn.GetAvgRating(i);
                allRatingsList.Add(i, rating);
            }
            ViewData["allRatingsList"] = allRatingsList;

            //if (searchStr == null)
            //{
            //    searchStr = "";
            //}
            List<Product> products = conn.GetSearchProducts(searchStr);
            ViewData["products"] = products;//Do something in view.
            ViewData["searchStr"] = searchStr;//Do something in view.
            return View();
        }

        public IActionResult InAccSearch(string searchStr)
        {

            //for rating
            float totalProducts = conn.GetTotalProducts();
            Dictionary<int, Rating> allRatingsList = new Dictionary<int, Rating>();
            for (int i = 1; i <= totalProducts; i++)
            {
                Rating rating = conn.GetAvgRating(i);
                allRatingsList.Add(i, rating);
            }
            ViewData["allRatingsList"] = allRatingsList;

            //if (searchStr == null)
            //{
            //    searchStr = "";
            //}
            List<Product> products = conn.GetSearchProducts(searchStr);
            ViewData["products"] = products;//Do something in view.
            ViewData["searchStr"] = searchStr;//Do something in view.
            return View();
        }
    }
}

