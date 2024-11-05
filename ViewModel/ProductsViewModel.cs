using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TawassolProject.Models;

namespace TawassolProject.ViewModel
{
    public class ProductsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.Now;

        public int Price { get; set; }

        public bool InStock { get; set; } = true;

        public string ProductImage { get; set; }
        public string? DepartmentName { get; set; }
        public string? CategoryName { get; set; }
        public int DepartmentId { get; set; }
        public int? CategoryId { get; set; }

        public IFormFile? SingleImageUpload { get; set; }

        public List<IFormFile>? MultiImageUpload { get; set; }
        public  Category Category { get; set; }
        public  Department Department { get; set; }

        public ICollection<ProductGallery> ProductGallery { get; set; }
        public IEnumerable<Product> ProductAlot { get; set; }
    }
}
