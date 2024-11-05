using System;
using TawassolProject.IRepository;

namespace TawassolProject.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IProductsRepository ProductsRepository { get; }
        IDepartmentsRepository DepartmentsRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        IShoppingCartRepository ShoppingCartRepository { get; }
        void SaveChanges();
    }
}
