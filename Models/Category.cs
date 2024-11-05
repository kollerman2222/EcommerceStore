using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryImage { get; set; }



        //relation
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public virtual ICollection<Product> Productss { get; set; }


    }
}
