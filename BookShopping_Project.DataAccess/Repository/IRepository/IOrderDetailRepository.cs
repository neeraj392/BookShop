using BookShopping_Project.Models;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface IOrderDetailRepository:IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
