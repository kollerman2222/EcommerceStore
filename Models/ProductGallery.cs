using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class ProductGallery
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
