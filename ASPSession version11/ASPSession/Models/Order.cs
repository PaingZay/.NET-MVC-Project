using System.ComponentModel.DataAnnotations;

namespace ASPSession.Models
{
    public class Order
    {
        public Order()
        {
            orderId++;
            //Product = new List<Product>();
        }
        [Required]
        public int orderId { get; set; }
        [Required]
        public int customerId { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public DateTime orderDate { get; set; }
        [Required]
        public int productQty { get; set; }
        [Required]
        public virtual Customer Customer { get; set; }
        // public virtual ICollection<Product> Product { get; set; }
        public virtual Product Product { get; set; }


    }
}
