using TawassolProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.Data;
using TawassolProject.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace TawassolProject.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;



        public OrderController(ApplicationDbContext context , UserManager<ApplicationUser> userManager , IUnitOfWork unitOfWork)
        {
            _context = context;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }



        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _unitOfWork.ShoppingCartRepository.GetShoppingCartTotalSum();
            var user = _userManager.GetUserId(User);
            order.UserId = user;
           
            _context.Orders.Add(order);
            _context.SaveChanges();

            var cartItems = _unitOfWork.ShoppingCartRepository.GetShoppingCartAllItemsAsync().Result;

            foreach (var product in cartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = product.Amount,
                    productId = product.product.Id,
                    OrderId = order.OrderId,
                    Price = product.product.Price
                };

                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();
        }


        [Route("/orders/orders")]
        public IActionResult Index()
        {
            var user = _userManager.GetUserId(User);
            var orders = _context.Orders.Where(o => o.UserId == user).Include(d=>d.OrderLines).ToList();
            var userOrder = orders.FirstOrDefault(o => o.UserId == user);
            var userOrderId = userOrder?.OrderId;
            var orderDetails = userOrderId != null? _context.OrderDetails.Where(o => o.OrderId == userOrderId).Include(p => p.product).ToList(): new List<OrderDetail>(); 
            var oooo = new ShoppingCartViewModel
            {
                OrderList = orders,
                OrderDetails = orderDetails
            };

            return View(oooo);
        }






        public IActionResult Checkout()
        {
            var getOrder = new Order();
            var orderSum = _unitOfWork.ShoppingCartRepository.GetShoppingCartTotalSum();
            getOrder.OrderTotal = orderSum;


            var order = new ShoppingCartViewModel
            {
                CartItems = _unitOfWork.ShoppingCartRepository.GetShoppingCartAllItemsAsync().Result,
                order = getOrder
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {

            var cartItems = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAllItemsAsync();
            if (cartItems.Count == 0)
            {
                ModelState.AddModelError("", "Cart is empty add products ");
            }

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                _unitOfWork.ShoppingCartRepository.ClearCart();
                _unitOfWork.SaveChanges();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }




        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Checkout is Complete , Thanks for your order";

            return View();
        }


    }





}

