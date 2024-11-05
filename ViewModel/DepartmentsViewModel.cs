using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TawassolProject.Models;

namespace TawassolProject.ViewModel
{
    public class DepartmentsViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string? DepartmentImage { get; set; }
        public IFormFile? SingleImageUpload { get; set; }

        public  ICollection<Category> Categorys { get; set; }

        public ICollection<Product> Productss { get; set; }

    }
}
