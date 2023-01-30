using System.Data.SqlClient;
using System.Data;
using System.Text;
using ASPSession.Models;
using ASPSession.Models;

namespace ASPSession.Connectors;

public class ShpCrtConnector : IConnector // a class that mainly deals with the database using CRUD statements
{
    string address;
    public ShpCrtConnector(IConfiguration cfg)
    {
        address = cfg.GetConnectionString("db_conn");
    }

    public bool IdentifyUserDB(string username, string password, out string? customerID) //returns true and customerID after matching username/password against the DB
    {
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT customerID FROM Customers WHERE username = @username AND password = @password;";
        
                cmd.Parameters.Add("@username", SqlDbType.NVarChar);
                cmd.Parameters.Add("@password", SqlDbType.NVarChar);
                cmd.Parameters["@username"].Value = username;
                cmd.Parameters["@password"].Value = password;
                try
                {
                    
                    customerID = Convert.ToInt32(cmd.ExecuteScalar()).ToString(); //if it throws an exception means user not found, if user found return true and provide ID to calling method
                    return customerID != "0"; //since above expression ToInt32 converts null to 0, hence this check will make sure 0 is not returned as customerID
                    
                }
                catch (Exception e) when (e is SqlException || e is NullReferenceException) 
                {
                    List<string> previous = File.ReadAllLines(@"Exceptions/Log.txt",Encoding.UTF8).ToList();
                    previous.Add(DateTime.Now.ToString("F"));
                    previous.Add(e.StackTrace!.ToString());
                    File.WriteAllLines(@"Exceptions/Log.txt", previous, Encoding.UTF8); ; //Logs stack trace into text file 
                    customerID = null; return false; 
                } 

            }
        }
    }

    public bool ProvideSessionDB(string customerID, string sessionID) // Insert session in DB so that it can be used to authenticate session cookies
    {
        using(SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Sessions (customerID, sessionID) VALUES (@customerID, @sessionID);";
                cmd.Parameters.Add("@customerID", SqlDbType.Int);
                cmd.Parameters.Add("@sessionID", SqlDbType.UniqueIdentifier);
                cmd.Parameters["@customerID"].Value = Convert.ToInt32(customerID);
                cmd.Parameters["@sessionID"].Value = Guid.Parse(sessionID);
                try
                {
                    return cmd.ExecuteNonQuery() == 1;  //will throw an exception if there is an old session, e.g., user could be logging in from another device

                }catch(SqlException e)
                {
                    List<string> previous = File.ReadAllLines(@"Exceptions/Log.txt", Encoding.UTF8).ToList();
                    previous.Add(DateTime.Now.ToString("F"));
                    previous.Add(e.StackTrace!.ToString());
                    File.WriteAllLines(@"Exceptions/Log.txt", previous, Encoding.UTF8); 
                    RemoveExpiredSessionDB(customerID);   // will remove old session
                }
                return ProvideSessionDB(customerID, sessionID); //and provide new session
            }
        }
    }


    public bool IsSessionValidDB(string customerID, string sessionID) // check userid and sessionid, if user has session stored in DB, hence current login session is valid
    {
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT sessionID FROM Sessions WHERE customerID = @customerID;";
                cmd.Parameters.Add("@customerID", SqlDbType.Int);
                cmd.Parameters["@customerID"].Value = Convert.ToInt32(customerID);
       
                try
                {
                    return cmd.ExecuteScalar().ToString() == sessionID; //checks if sessionID in DB matches cookiee sessionID

                }
                catch(Exception e) when (e is SqlException || e is NullReferenceException)
                {
                    List<string> previous = File.ReadAllLines(@"Exceptions/Log.txt", Encoding.UTF8).ToList();
                    previous.Add(DateTime.Now.ToString("F"));
                    previous.Add(e.StackTrace!.ToString());
                    File.WriteAllLines(@"Exceptions/Log.txt", previous, Encoding.UTF8); ; //Logs stack trace into text file 
                    return false;
                }
                    
            }
        }
    }

    public bool RemoveExpiredSessionDB(string customerID)
    {
        using(SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Sessions WHERE customerID = @customerID";
                cmd.Parameters.Add("@customerID", SqlDbType.Int);
                cmd.Parameters["@customerID"].Value = Convert.ToInt32(customerID);
                

                 try
                {
                    return cmd.ExecuteNonQuery() == 1;    //returns true if a row is deleted

                }catch(SqlException e)
                {
                    List<string> previous = File.ReadAllLines(@"Exceptions/Log.txt", Encoding.UTF8).ToList();
                    previous.Add(DateTime.Now.ToString("F"));
                    previous.Add(e.StackTrace!.ToString());
                    File.WriteAllLines(@"Exceptions/Log.txt", previous, Encoding.UTF8); ; //Logs stack trace into text file 
                    return false;
                }
              
                    
            }
        }
    }


    public List<Product> GetAllProducts()
    {
        List<Product> product = new List<Product>();
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            string sql = @"SELECT ProductID, ProductName, ProductPrice, ProductDesc, ProductIMG
                            FROM Product";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product p = new Product()
                {
                    ProductID = (int)reader["ProductID"],
                    ProductName = (string)reader["ProductName"],
                    ProductPrice = (decimal)reader["ProductPrice"],
                    ProductDesc = (string)reader["ProductDesc"],
                    ProductIMG = (string)reader["ProductIMG"]
                };
                product.Add(p);//add every single product to the product LIST by looping.
            }
        }
        return product;
    }

    //SEARCH QUERY

    public List<Product> GetSearchProducts(string searchStr)
    {
        List<Product> product = new List<Product>();
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            string sql = @"SELECT ProductID, ProductName, ProductPrice, ProductDesc, ProductIMG
                            FROM Product where ProductName like ('%" + searchStr + "%') or ProductDesc like ('%" + searchStr + "%') ";

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product p = new Product()
                {
                    ProductID = (int)reader["ProductID"],
                    ProductName = (string)reader["ProductName"],
                    ProductPrice = (decimal)reader["ProductPrice"],
                    ProductDesc = (string)reader["ProductDesc"],
                    ProductIMG = (string)reader["ProductIMG"]
                };
                product.Add(p);
            }
        }
        return product;
    }



    public float GetTotalProducts()
    {
        float totalProducts = 0;

        //toresume: outsource connectionstring
        //save before making it modular
        //C:\EASC\com_Gdipsa\MSVSe2022\DependencyInjection\EntityFrameworkDemo\bin\Debug\net6.0

        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            string sql = @"SELECT COUNT(productid) AS productCount from product";

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                totalProducts = (int)reader["productCount"];
            }
            conn.Close();

        }
        return totalProducts;
    }

    public Rating GetAvgRating(int ProductID)
    {
        Rating rating = new Rating();

    

        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();

            string sql = @"select AVG(ratingValue) as AvgRating from rating where ProductID=" + ProductID;

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                rating.ProductID = ProductID;

                if (reader["AvgRating"] == DBNull.Value)
                {
                    rating.ratingValue = 0;
                }
                else
                {
                    rating.ratingValue = (int)reader["AvgRating"];
                }

            }
            conn.Close();

        }
        return rating;
    }

    //or just delete this and do it the AJAX way?
    public Dictionary<int, Rating> GetUserRatings(int customerId)
    {
        Dictionary<int, Rating> userRatingsDict = new Dictionary<int, Rating>();

        

        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();

            string sql = @"select * from rating where customerId=" + customerId;

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Rating rating = new Rating();
                rating.ratingId = (int)reader["RatingId"];
                rating.ProductID = (int)reader["ProductId"];
                rating.customerId = (int)reader["customerId"];
                rating.ratingValue = (int)reader["RatingValue"];

                userRatingsDict.Add((int)reader["ProductId"], rating);

            }
            conn.Close();

        }
        return userRatingsDict;

    }

    public bool GetOrderByCustomer(string? customerID, ref List<Order> orders)
    {
        
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * from Orders where customerID= @customerID";
                cmd.Parameters.Add("@customerID", SqlDbType.Int);
                cmd.Parameters["@customerID"].Value = Convert.ToInt32(customerID);

                try
                {
                     using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            orderId = reader.GetInt32(0),
                            ProductID = reader.GetInt32(2),
                            orderDate = reader.GetDateTime(3),
                            productQty = reader.GetInt32(4)
                        };
                        orders.Add(order);
                    }
                        return true;
                }

                }
                catch (Exception e) when (e is SqlException || e is NullReferenceException)
                {
                    List<string> previous = File.ReadAllLines(@"Exceptions/Log.txt", Encoding.UTF8).ToList();
                    previous.Add(DateTime.Now.ToString("F"));
                    previous.Add(e.StackTrace!.ToString());
                    File.WriteAllLines(@"Exceptions/Log.txt", previous, Encoding.UTF8); ; //Logs stack trace into text file 
                    return false;
                }

               
            }
        }
    }

    public Guid GetActivationCodesByOrder(int orderID)/*these codes are needed to modify, since when I test,
                                                                           I use specified data to show if my program run or not.so if we want to generate 
                                                                           activation codes automatically, this method is needed to do some modification*/
    {
      
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT activationID FROM ActivationCodes where orderID= @orderID";
                cmd.Parameters.Add("@orderID", SqlDbType.Int);
                cmd.Parameters["@orderID"].Value = orderID;

                try
                {
                    return Guid.Parse(cmd.ExecuteScalar().ToString());

                }
                catch (Exception e) when (e is SqlException || e is NullReferenceException)
                {
                    List<string> previous = File.ReadAllLines(@"Exceptions/Log.txt", Encoding.UTF8).ToList();
                    previous.Add(DateTime.Now.ToString("F"));
                    previous.Add(e.StackTrace!.ToString());
                    File.WriteAllLines(@"Exceptions/Log.txt", previous, Encoding.UTF8); ; //Logs stack trace into text file 
                }
                return Guid.Empty;
              
            }
   
        }
    }

    public Product GetProductByID(int productID)
    {
        Product product = new Product();
        using (SqlConnection conn = new SqlConnection(address))
        {
            conn.Open();
            string q = string.Format(@"SELECT * FROM Product where ProductID='{0}'", productID);
            using (SqlCommand cmd = new SqlCommand(q, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        product = new Product
                        {
                            ProductID = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            ProductPrice = reader.GetDecimal(2),
                            ProductDesc = reader.GetString(3),
                            ProductIMG = reader.GetString(4)
                        };


                    }
                }
            }
            conn.Close();
        }
        return product;
    }


}
