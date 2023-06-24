using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping_Project.Models
{
   public class OrderDetail
    {
        public int id { get; set; }
        public int Orderid { get; set; }
        [ForeignKey("Orderid")]
        public OrderHeader OrderHeader { get; set; }
        public int Productid { get; set; }
        [ForeignKey("Productid")]
        public Product Product { get; set; }
        public int Count { get; set; }
        public double price { get; set; }
    }
}
