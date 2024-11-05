using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TawassolProject.Models;
using TawassolProject.ViewModel;

namespace TawassolProject.IRepository
{
    public interface IProductsRepository
    {

        Task<Product> GetByIdAsync(int? id);

        Task<IEnumerable<Product>> GetAllAsync();

        Task CreateAsync(ProductsViewModel pvm);

        string uploadFolderPublic { get; }

        Task<string> SingleImageUploadAsync(IFormFile image);

        Task<string> MultiImageUploadAsync(List<IFormFile> image, Product pro);
        void Delete(Product p);
    }
}
