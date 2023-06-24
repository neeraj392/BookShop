using BookShopping_Project.Models;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface ICompanyRepository:IRepository<Company>
    {
        void Update(Company company);
    }
}
