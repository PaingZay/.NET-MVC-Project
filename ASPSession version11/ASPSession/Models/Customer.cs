using System.ComponentModel.DataAnnotations;

namespace ASPSession.Models
{
    public class Customer
    {
        public Customer()
        {
            customerId++;
            Order = new List<Order>();
        }
        [Required]
        public int customerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Order> Order { get; set; }  
    }
}
