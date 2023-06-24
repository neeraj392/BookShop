using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;

namespace BookShopping_Project.DataAccess.Repository
{
    public class Unitofwork : IUnitofwork
    {
        private readonly ApplicationDbContext _context;
        public Unitofwork(ApplicationDbContext context)
        {
            _context = context;
            category = new CategoryRepository(_context);
            coverType = new CoverTypeRepository(_context);                
            Product = new ProductRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            Company = new CompanyRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
        }
        public ICategoryRepository category { get; private set; }
        public ICoverTypeRepository coverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
