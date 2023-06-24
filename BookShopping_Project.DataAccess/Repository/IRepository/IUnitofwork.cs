
namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface IUnitofwork
    {
        ICategoryRepository category { get; }
        ICoverTypeRepository coverType { get; }
        IProductRepository Product { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }

        void Save();
    }
}
