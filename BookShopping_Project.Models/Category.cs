using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models
{
   public  class Category
    {
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        public string Name { get; set; }
    }
}
