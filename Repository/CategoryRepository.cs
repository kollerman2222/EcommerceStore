using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using TawassolProject.Data;
using TawassolProject.IRepository;
using TawassolProject.Models;
using TawassolProject.ViewModel;
using System.Linq;

namespace TawassolProject.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;
        public CategoryRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/categories";
        }

        public string uploadFolderPublic { get { return _uploadFolderPath; } }

        public async Task CreateAsync(CategoriesViewModel cvm)
        {
            Category newCategory = new()
            {
                Name = cvm.Name,
                CategoryImage = await SingleImageUploadAsync(cvm.SingleImageUpload),
                DepartmentId=cvm.DepartmentId

            };
            _context.Categories.Add(newCategory);

        }

        public void Delete(Category c)
        {
            _context.Categories.Remove(c);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.Include(d=>d.Department).Include(c => c.Productss).ToListAsync();
            return categories;
        }

        public async Task<Category> GetByIdAsync(int? id)
        {
            var categories = await _context.Categories.Include(d => d.Department).Include(c => c.Productss).FirstOrDefaultAsync(g => g.Id == id);
            return categories;
        }

        public List<Category> GetAllByDepartmentId(int? id)
        {
            var categories = _context.Categories.Where(c=>c.DepartmentId==id).ToList();
            return categories;
        }


        public async Task<string> SingleImageUploadAsync(IFormFile image)
        {

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await image.CopyToAsync(stream);

            return imageName;
        }
    }
}
