using ASPSession.Models;

namespace ASPSession.Models.ViewModels
{
        public class CartItem
        {
                public int ProductId { get; set; }
                public string ProductName { get; set; }
                public int Quantity { get; set; }
                public decimal Price { get; set; }
                public decimal Total
                {
                        get { return Quantity * Price; }
                }
                public string Image { get; set; }

                public CartItem()
                {
                }

                public CartItem(Product product)
                {
                        ProductId = product.ProductID;
                        ProductName = product.ProductName;
                        Price = product.ProductPrice;
                        Quantity = 1;
                        Image = product.ProductIMG;
                }

        }
}
