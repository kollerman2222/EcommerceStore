using System.Collections.Generic;
using TawassolProject.Models;

namespace TawassolProject.ViewModel
{
    public class ProductShopViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int TotalPages { get; set; }

        public int Page { get; set; }

        public string? depName { get; set; }
    }
}
