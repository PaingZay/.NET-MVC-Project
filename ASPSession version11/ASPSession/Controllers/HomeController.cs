
using ASPSession.Connectors;
using ASPSession.Models;
using ASPSession.Models.ViewModels;
using ASPSession.Security;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPSession.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ISecurity sec;
        private readonly IConnector conn;
        private readonly IHttpContextAccessor context;

        public HomeController(ISecurity sec, IConnector conn, IHttpContextAccessor context)
        {
            this.sec = sec;
            this.conn = conn;
            this.context = context;

        }

        public IActionResult Index()
        {
            
            //for rating
            float totalProducts = conn.GetTotalProducts();
            Dictionary<int,Rating> allRatingsList = new Dictionary<int,Rating>();
            for (int i = 1; i <= totalProducts; i++)
            {
                Rating rating = conn.GetAvgRating(i);
                allRatingsList.Add(i,rating);
            }
            ViewData["allRatingsList"] = allRatingsList;

            //for search
            List<Product> products = conn.GetAllProducts();
            ViewData["products"] = products;

            return View();
        }

        public IActionResult Search(string searchStr)
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
            ViewData["username"] = context.HttpContext!.Session.GetString("username");
            return View("Main");
        }

        public IActionResult Main()
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

            //for search
            List<Product> products = conn.GetAllProducts();
            ViewData["products"] = products;
            ViewData["username"] = context.HttpContext!.Session.GetString("username");

            return View();
        }


        public IActionResult Cart()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };
            ViewData["username"] = context.HttpContext!.Session.GetString("username");

            return View(cartVM);
        }

        public IActionResult MyPurchases()
        {

            List<Order> orders = new List<Order>();
            if(conn.GetOrderByCustomer(context.HttpContext!.Session.GetString("customerID"), ref orders )) //if returns false, skips the following processes and return empty view
            {
                Dictionary<int, Guid> activationCodes =
                 new Dictionary<int,Guid>();
                List<Product> ProductList = new List<Product>();
                for (int i = 0; i < orders.Count; i++)
                {
                    int orderID = orders[i].orderId;
                    activationCodes[orderID] = conn.GetActivationCodesByOrder(orderID);
                    int productID = orders[i].ProductID;
                    Product product = conn.GetProductByID(productID);
                    ProductList.Add(product);
                }
                ViewData["Orders"] = orders;
                ViewData["ActivationCode"] = activationCodes;
                ViewData["Product"] = ProductList;
            }
        
            return View();
        }




        public IActionResult Privacy()
        {
            if (!sec.Authenticate()) return RedirectToAction("AccessDenied");
            else return View();
        }

        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

  
        public IActionResult AccessDenied()
        {
            return View();
        }
    }


}