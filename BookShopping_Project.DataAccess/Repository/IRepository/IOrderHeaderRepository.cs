using BookShopping_Project.Models;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        void update(OrderHeader orderHeader);
    }
}
