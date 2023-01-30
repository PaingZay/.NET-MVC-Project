using Microsoft.AspNetCore.Mvc;
using ASPSession.Security;

namespace ASPSession.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //does not cache login page after logged-in, forces browser to request from server a new page, server will check and prevent user from viewing login page if already logged in
    public class UserAccessController : Controller
    {
        ISecurity sec;
        public UserAccessController(ISecurity sec)
        {
            this.sec = sec;
        }

        
        public IActionResult LoginPage()
        {
            if (sec.Authenticate()) return RedirectToAction("Main","Home");
            return View();
        }

    

        public IActionResult Invalid(int id)
        {
            if (sec.Authenticate()) return RedirectToAction("Main", "Home");
            return View();
        }


        public IActionResult Logout()
        {
            sec.RemoveAuthentication();
            return RedirectToAction("LoginPage","UserAccess");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Verify(string username, string password)  //takes in data from ajax script to verify user
        {
             return Json(Login(username, password)); // returns boolean value in a json object to the view
           
        }

        private bool Login(string username, string password)
        {
           
            string? customerID;
            if (sec.LoginAuthentication(username, password, out customerID)) //authenticate credentials, will provide customerID if true
            {
                sec.ProvideSession(username, customerID!); //if authenticated, provide with a session to allow access to authorized pages
                return true;
            }
            else return false;

        }

    }
}
