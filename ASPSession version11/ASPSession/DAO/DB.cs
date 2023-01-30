using ASPSession.Models;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace ASPSession
{
    public class DB
    {
        private string connStr;

        public DB(string connStr)
        {
            this.connStr = connStr;
        }
      
    }
}
