using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using TawassolProject.Data;
using TawassolProject.IRepository;
using TawassolProject.ViewModel;
using TawassolProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TawassolProject.Repository
{
    public class ProductsRepository:IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolderPath;

        public ProductsRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFolderPath = $"{_webHostEnvironment.WebRootPath}/uploads/products";
        }

        public string uploadFolderPublic { get { return _uploadFolderPath; } }

        public async Task CreateAsync(ProductsViewModel pvm)
        {
            Product newProduct = new()
            {
                Name = pvm.Name,
                Description = "adadadada",
                Brand = "test",
                Price = 100,
                DepartmentId = 2,
                CategoryId = 2,
                ProductImage = await SingleImageUploadAsync(pvm.SingleImageUpload),

            };

            var addedproduct = await _context.Products.AddAsync(newProduct);

            await MultiImageUploadAsync(pvm.MultiImageUpload,newProduct);

        }

        public void Delete(Product p)
        {
            _context.Products.Remove(p);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products.Include(d=>d.Department).Include(c=>c.Category).ToListAsync();
            return products;
        }

        public async Task<Product> GetByIdAsync(int? id)
        {
            var product = await _context.Products.Include(d=>d.Department).Include(c=>c.Category).Include(g=>g.Gallery).FirstOrDefaultAsync(g=>g.Id==id);
            return product;
        }

     

        public async Task<string> SingleImageUploadAsync(IFormFile image)
        {

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

            using var stream = File.Create(imageSaveLocation);
            await image.CopyToAsync(stream);

            return imageName;
        }


        public async Task<string> MultiImageUploadAsync(List<IFormFile> image , Product pro)
        {
            string UniqueimageName = string.Empty;
            foreach(IFormFile file in image)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                var imageSaveLocation = Path.Combine(_uploadFolderPath, imageName);

                using var stream = File.Create(imageSaveLocation);
                await file.CopyToAsync(stream);

                ProductGallery pgg = new()
                {
                    Product=pro,
                    ImageURL= imageName
                };

                UniqueimageName = imageName;
                await _context.Galleries.AddAsync(pgg);


            }

            return UniqueimageName;
        }
    }
}
