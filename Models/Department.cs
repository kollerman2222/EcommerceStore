using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DepartmentImage { get; set; }



        //relation
        public virtual ICollection<Category> Categorys { get; set; }

        public virtual ICollection<Product> Productss { get; set; }


    }
}
