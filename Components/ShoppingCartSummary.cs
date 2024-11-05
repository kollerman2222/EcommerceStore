using TawassolProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.UnitOfWork;

namespace catoandsubtest.Components
{
    public class ShoppingCartSummary:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartSummary(IUnitOfWork unitOfWork)
        {
           
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var cartItems = _unitOfWork.ShoppingCartRepository.GetShoppingCartAllItemsAsync();

            var scvm = new ShoppingCartViewModel
            {
                CartItems = cartItems.Result,
                ShoppingCartTotal = _unitOfWork.ShoppingCartRepository.GetShoppingCartTotalSum(),
            };

            return View(scvm);
        }
    }
}
