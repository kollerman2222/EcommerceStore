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

namespace TawassolProject.Repository
{
    public class DepartmentsRepository:IDepartmentsRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;
        public DepartmentsRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) 
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/departments";
        }

        public string uploadFolderPublic { get { return _uploadFolderPath; } }

        public async Task CreateAsync(DepartmentsViewModel dvm)
        {
            Department newDepartment = new()
            {
                Name = dvm.Name,
                DepartmentImage = await SingleImageUploadAsync(dvm.SingleImageUpload),

            };
            _context.Departments.Add(newDepartment);

        }

        public void Delete(Department d)
        {
            _context.Departments.Remove(d);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var departments = await _context.Departments.Include(c => c.Categorys).ToListAsync();
            return departments;
        }

        public async Task<Department> GetByIdAsync(int? id)
        {
            var Department = await _context.Departments.Include(c => c.Categorys).Include(p=>p.Productss).FirstOrDefaultAsync(g => g.Id == id);
            return Department;
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
