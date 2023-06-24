using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
namespace BookShopping_Project.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void update(OrderHeader orderHeader)
        {
            _context.Update(orderHeader);
            
        }
    }
}
