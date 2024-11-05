using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TawassolProject.Data;
using TawassolProject.IRepository;
using TawassolProject.Repository;

namespace TawassolProject.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContext;

        public IDepartmentsRepository DepartmentsRepository { get; private set; }
        public IProductsRepository ProductsRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment , IHttpContextAccessor httpContext)
        { 
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContext;
            DepartmentsRepository = new DepartmentsRepository(_context,webHostEnvironment);
            ProductsRepository = new ProductsRepository(_context,webHostEnvironment);
            CategoryRepository = new CategoryRepository(_context,webHostEnvironment);
            ShoppingCartRepository = new ShoppingCartRepository(_context, _httpContext);
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }




    }
}
