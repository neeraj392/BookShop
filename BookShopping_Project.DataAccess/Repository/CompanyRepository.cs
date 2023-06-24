using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
namespace BookShopping_Project.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Company company)
        {
            _context.Update(company);
        }
    }
}
