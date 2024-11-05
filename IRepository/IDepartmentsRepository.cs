using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TawassolProject.Models;
using TawassolProject.ViewModel;

namespace TawassolProject.IRepository
{
    public interface IDepartmentsRepository
    {
        Task<Department> GetByIdAsync(int? id);

        Task<IEnumerable<Department>> GetAllAsync();

        Task CreateAsync(DepartmentsViewModel pvm);

        string uploadFolderPublic { get; }

        Task<string> SingleImageUploadAsync(IFormFile image);

        void Delete(Department d);
    }
}
