using System;
using ASPSession.Models;
using Microsoft.Data.SqlClient;

namespace ASPSession.Data
{
    public class ProductData
    {
        string address;
        public ProductData(IConfiguration cfg)
        {
            address = cfg.GetConnectionString("db_conn");
        }
       
    }
}

