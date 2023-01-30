using ASPSession.Connectors;
using ASPSession.Models;
using ASPSession.Security;
using Microsoft.AspNetCore.Mvc;

namespace ASPSession.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly ISecurity sec;
        private readonly IConnector conn;
        private readonly EFConnector _context;

        public ProductController(ISecurity sec, IConnector conn, EFConnector context)
        {
            this.sec = sec;
            this.conn = conn;
            _context = context;

        }


        public IActionResult Index()
        {

            return View(_context.Product.OrderBy(p => p.ProductID));
        }

        /*     public IActionResult Index()
             {
                 if (!sec.Authenticate()) return View("AccessDenied");
                 List<Product> products = conn.GetAllProducts();
                 ViewData["products"] = products;

                 return View();
             }
     */


    }
}

