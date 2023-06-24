using BookShopping_Project.Models;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface ICategoryRepository:IRepository<Category>
    {
        void update(Category category);
    }
}
