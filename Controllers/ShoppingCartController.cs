using TawassolProject.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.Models;
using Microsoft.AspNetCore.Authorization;
using TawassolProject.UnitOfWork;

namespace TawassolProject.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            
            var cartItems = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAllItemsAsync();

            var scvm = new ShoppingCartViewModel
            {
                CartItems = cartItems,
                ShoppingCartTotal = _unitOfWork.ShoppingCartRepository.GetShoppingCartTotalSum(),
            };

            return View(scvm);

        }

        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if(product != null)
            {
               await _unitOfWork.ShoppingCartRepository.AddToCartAsync(product);
               _unitOfWork.SaveChanges();
            }

            return RedirectToAction("Index");
        }
           


        public async Task<IActionResult> RemoveFromShoppingCart(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if (product != null)
            {
                await _unitOfWork.ShoppingCartRepository.RemoveFromCartAsync(product);
                _unitOfWork.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> increaseproductamount(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if (product != null)
            {
                int currentAmount = await _unitOfWork.ShoppingCartRepository.AddToCartAsync(product);
                _unitOfWork.SaveChanges();
                var totalAmount = _unitOfWork.ShoppingCartRepository.GetShoppingCartTotalSum();
                var singleTotalAmount = _unitOfWork.ShoppingCartRepository.GetShoppingCartSingleTotalSum(product);
                return Json(new { currentAmount , totalAmount , singleTotalAmount });
            }

            return BadRequest();
        }


        public async Task<IActionResult> decreaseproductamount(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if (product != null)
            {
                int currentAmount = await _unitOfWork.ShoppingCartRepository.RemoveFromCartAsync(product);
                _unitOfWork.SaveChanges();
                var totalAmount = _unitOfWork.ShoppingCartRepository.GetShoppingCartTotalSum();
                var singleTotalAmount = _unitOfWork.ShoppingCartRepository.GetShoppingCartSingleTotalSum(product);
                return Json(new { currentAmount , totalAmount , singleTotalAmount });
            }
            return BadRequest();

        }

        
        public IActionResult ClearCart()
        {
            _unitOfWork.ShoppingCartRepository.ClearCart();
            _unitOfWork.SaveChanges();
            return Json("Cart is empty");
        }


    }
}
