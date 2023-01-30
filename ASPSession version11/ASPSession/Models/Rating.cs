namespace ASPSession.Models;

public class Rating
{
    public int ratingId { get; set; }
    public int ProductID { get; set; }
    public int customerId { get; set; }
    public int ratingValue { get; set; }

}
