
using BookShopping_Project.Models;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);

    }
}
