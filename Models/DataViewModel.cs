
using FoolProof.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class DataViewModel
    {
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }
        public ICollection<Department> Departmentss { get; set; }

        public string  DepImageVM { get; set; }

        [Display(Name = "Category")]

        public int? CategoryID { get; set; }
        public ICollection<Category>? Categoryss { get; set; } = new List<Category>();


        public Department Departmenttt { get; set; }

        public Product Producttt { get; set; }
        
      
        [Required]
        public string Name { get; set; }

        public ICollection<ProductGallery> pgs { get; set; }

        public List<IFormFile> Images { get; set; }


        
        public IFormFile SingleImage { get; set; }



        public IEnumerable<Product> ProductAlot { get; set; }


        public string MainProductImage { get; set; }





    }
}
