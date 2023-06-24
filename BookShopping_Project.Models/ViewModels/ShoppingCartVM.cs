using System.Collections.Generic;

namespace BookShopping_Project.Models.ViewModels
{
   public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> Listcart { get; set; }
        public OrderHeader orderHeader { get; set; }

    }
}
    