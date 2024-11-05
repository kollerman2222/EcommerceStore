using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.Data;
using TawassolProject.IRepository;
using TawassolProject.Models;

namespace TawassolProject.Repository
{
    public class ShoppingCartRepository:IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ShoppingCartRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCartId()
        {
            string cartId = _httpContextAccessor.HttpContext.Session.GetString("CartId");

            if(string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                _httpContextAccessor.HttpContext.Session.SetString("CartId", cartId);

            }

            return cartId;
        }



        public async Task<int> AddToCartAsync(Product pro)
        {
            var newAmount = 0;
            var cartId = GetCartId();
            var shoppingCart = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.product.Id == pro.Id && s.ShoppingCartId == cartId);
            if (shoppingCart == null)
            {
                 shoppingCart = new ShoppingCartItem
                {
                    ShoppingCartId = GetCartId(),
                    product = pro,
                    Amount = 1
                };
                
                _context.ShoppingCartItems.Add(shoppingCart);
                newAmount = shoppingCart.Amount;
                return newAmount;
            }
            else
            {
                shoppingCart.Amount++;
                newAmount = shoppingCart.Amount;
                return newAmount;
            }
        }

        public async Task<int> RemoveFromCartAsync(Product pro)
        {
            var newAmount = 0;
            var cartId = GetCartId();
            var shoppingCart = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.product.Id == pro.Id && s.ShoppingCartId == cartId);
            if (shoppingCart != null)
            {
                if (shoppingCart.Amount > 1)
                {
                    shoppingCart.Amount--;
                    newAmount = shoppingCart.Amount;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCart);
                }
            }

            return newAmount;

        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartAllItemsAsync()
        {
            var cartId = GetCartId();
            var shoppingCart = await _context.ShoppingCartItems.Where(c => c.ShoppingCartId == cartId).Include(s => s.product).ToListAsync();
            return shoppingCart;
        }

        public decimal GetShoppingCartTotalSum()
        {
            var cartId = GetCartId();
            var cartTotalSum =  _context.ShoppingCartItems.Where(c => c.ShoppingCartId == cartId).Select(c => c.product.Price * c.Amount).Sum();
            return cartTotalSum;
        }
        public decimal GetShoppingCartSingleTotalSum(Product pro)
        {
            var cartId = GetCartId();
            var cartTotalSum = _context.ShoppingCartItems.Where(s => s.product.Id == pro.Id && s.ShoppingCartId == cartId).Select(c => c.product.Price * c.Amount).Sum();
            return cartTotalSum;
        }

        public void ClearCart()
        {
            var cartId = GetCartId();
            var shoppingCart = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == cartId);
            _context.ShoppingCartItems.RemoveRange(shoppingCart);

        }





    }
}
