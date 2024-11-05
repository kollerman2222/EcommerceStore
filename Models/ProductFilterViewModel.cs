using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class ProductFilterViewModel
    {
        public ProductFilterViewModel()
        {
            SelectedDepartments = new List<string>();
            SelectedCategories = new List<string>();
        }
        public List<Product> Products { get; set; }
        public List<Department> Departments { get; set; }
        public List<Category> Categories { get; set; }

        public List<string> SelectedDepartments { get; set; }
        public List<string> SelectedCategories { get; set; }

        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public PageInfo PageInfo { get; set; }

    }
}
