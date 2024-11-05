using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.Data;
using TawassolProject.Models;
using TawassolProject.UnitOfWork;
using TawassolProject.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Collections.Specialized.BitVector32;

namespace TawassolProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
        }


       
        public IActionResult HomePage()
        {
            var model = new DataViewModel
            {

                Departmentss = _context.Departments.ToList(),
                Categoryss = _context.Categories.ToList(),
                ProductAlot = _context.Products.ToList()
            };

            return View(model);

            
        }


        [HttpGet]
        public async Task<IActionResult> searchproduct(string pname)
        {
            if (string.IsNullOrEmpty(pname))
            {
                // Set a default value for the search term in the session
                HttpContext.Session.SetString("searchterm", string.Empty);
            }
            else
            {
                HttpContext.Session.SetString("searchterm", pname);
            }

            var products = await _context.Products.Where(p => p.Name.Contains(pname)).ToListAsync();
            
            var psvm = new ProductShopViewModel
            {
                Products = products,
                Departments = _context.Departments.ToList(),
                Categories = _context.Categories.ToList()

            };

            return View("Index5",psvm);
        }


        

        public async Task<IActionResult> autocompletesearch(string query)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(query))
                .Select(p => new
                {
                    value = p.Name,  // value and data name can be anything but i choose chose according to the docs of plugin
                    data= "" // i can put anything if i wanted
                    
        })
                .ToListAsync();

            return Json(new { query , suggestions = products }); // param names according to doc of plugin
        }



        [Route("/products/shop")]
        [Route("/products/shop/{catid}")]
        public async Task<IActionResult> Index5(string depName ,int? catid , int pageNumber = 1)
        {

            var products = await _unitOfWork.ProductsRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(depName))
            {
                products = products.Where(x => x.Department?.Name == depName);
            }
            if(string.IsNullOrEmpty(depName) && catid != null)
            {
                products = products.Where(x => x.CategoryId == catid);
            }
           
            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / 6);
            var pagination = products.Skip((pageNumber - 1) * 6).Take(6).ToList();

            var model = new ProductShopViewModel
            {
                Departments = await _unitOfWork.DepartmentsRepository.GetAllAsync(),
                Categories = await _unitOfWork.CategoryRepository.GetAllAsync(),
                Products = pagination,
                TotalPages = totalPages,
                Page = pageNumber,
                depName = depName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProductsByCategory(List<int?> categories , string pricerange , string depName, int pageNumber = 1)
        {

            var products = await _unitOfWork.ProductsRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(depName))
            {
                products = products.Where(x => x.Department?.Name == depName);
            }
            if (categories!= null && categories.Count>0)
            {
                products = products.Where(p => categories.Contains(p.CategoryId));
            }
            if (!string.IsNullOrEmpty(pricerange))
            {
                if(pricerange == "min500")
                {
                    products = products.Where(p => p.Price > 500);
                }
                else if (pricerange == "max500")
                {
                    products = products.Where(p => p.Price < 500);
                }
                else if (pricerange == "max1000")
                {
                    products = products.Where(p => p.Price < 1000);
                }
                else if (pricerange == "min1000")
                {
                    products = products.Where(p => p.Price > 1000);
                }

            }
            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / 6);
            var pagination = products.Skip((pageNumber - 1) * 6).Take(6).ToList();
            var filterProducts = products.ToList();
            var psvm = new ProductShopViewModel
            {
                Departments = await _unitOfWork.DepartmentsRepository.GetAllAsync(),
                Categories = await _unitOfWork.CategoryRepository.GetAllAsync(),
                Products = pagination,
                TotalPages = totalPages,
                Page=pageNumber,
                depName = depName


            };
            return PartialView("_productShopPartial", psvm);
        }


        

        
    }
}
