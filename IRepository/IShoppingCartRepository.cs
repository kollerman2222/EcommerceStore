using System.Collections.Generic;
using System.Threading.Tasks;
using TawassolProject.Models;

namespace TawassolProject.IRepository
{
    public interface IShoppingCartRepository
    {
        string GetCartId();
        Task<int> AddToCartAsync(Product pro);
        Task<int> RemoveFromCartAsync(Product pro);
        Task<List<ShoppingCartItem>> GetShoppingCartAllItemsAsync();
        decimal GetShoppingCartTotalSum();
        decimal GetShoppingCartSingleTotalSum(Product pro);
        void ClearCart();

    }
}
