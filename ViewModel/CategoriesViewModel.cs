using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TawassolProject.Models;

namespace TawassolProject.ViewModel
{
    public class CategoriesViewModel
    {
        public int? Id { get; set; }
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string CategoryImage { get; set; }
        public string DepartmentName { get; set; }

        public IFormFile? SingleImageUpload { get; set; }
        public Department Department { get; set; }

        public Category category { get; set; }

        public  ICollection<Product> Productss { get; set; }
        public  IEnumerable<Category> Categorys { get; set; }


    }
}
