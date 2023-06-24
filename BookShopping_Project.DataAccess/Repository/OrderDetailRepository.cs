using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
namespace BookShopping_Project.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }       
        public void Update(OrderDetail orderDetail)
        {
            _context.Update(orderDetail);
        }
    }
}
