using System.ComponentModel.DataAnnotations;
namespace BookShopping_Project.Models
{
   public class CoverType
   {
        public int Id { get; set; }
        [Display(Name = "CoverType Name")]
        [Required]
        public string Name { get; set; }
   }
}
