using ASPSession.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace ASPSession.Controllers
{
   
    public class OrderHistoryController : Controller
    {
        private DB db;
        public OrderHistoryController(IConfiguration cfg)
        {
            db = new DB(cfg.GetConnectionString("db_conn"));
        }
     
    }
}
