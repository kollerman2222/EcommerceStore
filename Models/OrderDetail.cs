using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int productId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public virtual Product product { get; set; }
        public virtual Order Order { get; set; }

   



    }
}
