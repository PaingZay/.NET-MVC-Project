
using ASPSession.Models;
using ASPSession.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPSession.Connectors
{
    public interface IConnector 
    {
        public bool IdentifyUserDB(string username, string password, out string? customerID);

        public bool ProvideSessionDB(string customerID, string sessionID);

        public bool IsSessionValidDB(string customerID, string sessionID);

        public bool RemoveExpiredSessionDB(string customerID);

        public List<Product> GetAllProducts();
        public List<Product> GetSearchProducts(string searchStr);

        public float GetTotalProducts();
        public Rating GetAvgRating(int ProductID);
        public Dictionary<int, Rating> GetUserRatings(int customerId);

        public Product GetProductByID(int productID);

        public Guid GetActivationCodesByOrder(int orderID);

        public bool GetOrderByCustomer(string? customerID, ref List<Order> orders);







    }
}
