using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookShopping_Project.Models
{
   public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }

        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price100 { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }
        [Display(Name = "Category")]

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        public Category Category { get; set; }
        [Display(Name = "CoverType")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }

    }
}
