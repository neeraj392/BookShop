
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookShopping_Project.Models.ViewModels
{
  public class ProductVM
    {
        public Product product { get; set; }
        public IEnumerable <SelectListItem> covertypelist { get; set; }
        public IEnumerable<SelectListItem> categorylist { get; set; }

    }
}
