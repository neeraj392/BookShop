using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using System.Linq;

namespace BookShopping_Project.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var ProductInDb = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if(ProductInDb!=null)
            {
               if(product.ImageUrl!= "")
                 ProductInDb.ImageUrl = product.ImageUrl;
                ProductInDb.Title = product.Title;
                ProductInDb.Description = product.Description;
                ProductInDb.Author = product.Author;
                ProductInDb.ISBN = product.ISBN;
                ProductInDb.ListPrice = product.ListPrice;
                ProductInDb.Price50 = product.Price50;
                ProductInDb.Price100 = product.Price100;
                ProductInDb.Price = product.Price;
                ProductInDb.CategoryId = product.CategoryId;
                ProductInDb.CoverTypeId = product.CoverTypeId;
            }
            _context.Update(ProductInDb);
        }
    }
}
