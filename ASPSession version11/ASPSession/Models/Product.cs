using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPSession.Models
{
        public class Product
        {
            public Product()
            {
                ProductID++;
                Cart = new List<Cart>();
                Order = new List<Order>();
            }
            [Required]
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            //public int Quantity { get; set; }
            public decimal ProductPrice { get; set; }
            public string ProductDesc { get; set; }
            public string ProductIMG { get; set; }
            public virtual ICollection<Cart> Cart { get; set; }
            public virtual ICollection<Order> Order { get; set; }


     
    

}
}
