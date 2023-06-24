using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;

namespace BookShopping_Project.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepository(ApplicationDbContext context):base(context)
        {
            _context = context; 
        }
        public void update(CoverType coverType)
        {
            _context.Update(coverType);
        }
    }
}
