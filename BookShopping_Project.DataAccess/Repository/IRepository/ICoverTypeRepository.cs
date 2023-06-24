using BookShopping_Project.Models;
namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface ICoverTypeRepository:IRepository<CoverType>
    {
        void update(CoverType coverType);

    }
}
