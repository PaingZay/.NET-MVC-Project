
using ASPSession.Models;
using System.Data.SqlClient;


namespace ASPSession.Data {
    public class RatingData
    {
        string address;
        public RatingData(IConfiguration cfg)
        {
            address = cfg.GetConnectionString("db_conn");
        }
        

        //public static int GetRating(int ProductID, int userId)
        //{

        //}

        //public static int SetRating(int ProductID, int userId, int ratingValue)
        //{

        //}
    }


}
