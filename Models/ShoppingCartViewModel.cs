using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TawassolProject.Models
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public List<ShoppingCartItem> CartItems { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public Order order { get; set; }
        public IEnumerable<Order>? OrderList { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public string CartId { get; set; }


    }
}
