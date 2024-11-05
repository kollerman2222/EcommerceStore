using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TawassolProject.Models;
using TawassolProject.ViewModel;

namespace TawassolProject.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int? id);
        List<Category> GetAllByDepartmentId(int? id);

        Task<IEnumerable<Category>> GetAllAsync();

        Task CreateAsync(CategoriesViewModel cvm);

        string uploadFolderPublic { get; }

        Task<string> SingleImageUploadAsync(IFormFile image);

        void Delete(Category c);
    }
}
