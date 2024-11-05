using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public DateTime AddedOn { get; set; }

        public int Price { get; set; }

        public bool  InStock { get; set; }

        

        public string ProductImage { get; set; }





        //relation
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProductGallery> Gallery { get; set; }


        public Product()
        {
            InStock = true;

            AddedOn = DateTime.Now;
        }



    }
}
