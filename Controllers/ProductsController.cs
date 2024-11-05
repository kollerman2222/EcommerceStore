using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.Data;
using TawassolProject.IRepository;
using TawassolProject.Models;
using TawassolProject.UnitOfWork;
using TawassolProject.ViewModel;
using static TawassolProject.Contants.Permissions;

namespace TawassolProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hosting , IUnitOfWork unitOfWork)
        {
            _context = context;
            _hosting = hosting;
            _unitOfWork = unitOfWork;
        }

        [Route("/products/main")]
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            var productslist = await  _unitOfWork.ProductsRepository.GetAllAsync();

            var productsListt = productslist.Select(p => new ProductsViewModel
            {
                // Map properties from Product to ProductsViewModel
                Id = p.Id,
                Name = p.Name,
                DepartmentName = p.Department != null ? p.Department.Name : "No Department",
                CategoryName = p.Category != null ? p.Category.Name : "No Category",
                ProductImage=p.ProductImage
                // Other necessary mappings
            }).ToList();
            return View(productsListt);
        }


        public async Task<IActionResult> Details(int? id )
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            
            var viewmodel = new ProductsViewModel
            {
               Name= product.Name,
               Description= product.Description,
               DepartmentId= product.DepartmentId,
               CategoryId=product.CategoryId,
               Brand=product.Brand,
               Price=product.Price,
               ProductImage=product.ProductImage,
               ProductGallery = product.Gallery,
               ProductAlot=await _unitOfWork.ProductsRepository.GetAllAsync(),
             };

            return View(viewmodel);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.DepartmentId = new SelectList(await _unitOfWork.DepartmentsRepository.GetAllAsync(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(await _unitOfWork.CategoryRepository.GetAllAsync(), "Id", "Name");

            return View();
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsViewModel pvm)
        {

            if (!ModelState.IsValid)
            {
                
                return View("Create");
            }

            await _unitOfWork.ProductsRepository.CreateAsync(pvm);
           
            
            return View();
        }





        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }


            var viewmodel = new ProductsViewModel
            {
               Id= product.Id,
               Name = product.Name,
               Description = product.Description,
               Brand = product.Brand,
               Price = product.Price,
               ProductImage = product.ProductImage,
               ProductGallery = product.Gallery,
               DepartmentId = product.DepartmentId,
               CategoryId = product.CategoryId,
               DepartmentName=product.Department.Name,
               CategoryName=product.Category.Name,
                
            };


            ViewBag.CategoryId = new SelectList(_unitOfWork.CategoryRepository.GetAllByDepartmentId(viewmodel.DepartmentId), "Id", "Name", viewmodel.CategoryId);
            ViewBag.DepartmentId = new SelectList(await _unitOfWork.DepartmentsRepository.GetAllAsync(), "Id", "Name", viewmodel.DepartmentId);
            return View(viewmodel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ProductsViewModel pvm)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                
                return View("Edit");
            }

            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            string oldSingleImage = product.ProductImage;

            product.Price = pvm.Price;
            product.Description = pvm.Description;
            product.Name = pvm.Name;
            product.Brand = pvm.Brand;
            product.CategoryId = pvm.CategoryId;
            product.DepartmentId = pvm.DepartmentId;
            if(pvm.SingleImageUpload != null)
            {
                product.ProductImage = await _unitOfWork.ProductsRepository.SingleImageUploadAsync(pvm.SingleImageUpload);
                var oldImageDelete = Path.Combine(_unitOfWork.ProductsRepository.uploadFolderPublic, oldSingleImage);
                System.IO.File.Delete(oldImageDelete);
            }
            if(pvm.MultiImageUpload != null)
            {
                await _unitOfWork.ProductsRepository.MultiImageUploadAsync(pvm.MultiImageUpload, product);
            }

              
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductsRepository.Delete(product);

            return View("Index");
        }



 




        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }



        public IActionResult GetCategory(int depid)
        {
            var cat = _context.Categories.Where(c => c.DepartmentId == depid).ToList();
            return Ok(cat);
        }




        public IActionResult ProductInStockStatus(int? id)
        {

            var ppp = _context.Products.Find(id);

            ppp.InStock = !ppp.InStock;

            _context.SaveChanges();

            return Ok();
        }




        public IActionResult DeleteProductImageAjax(int? id)
        {
            string uploadfolderr = Path.Combine(_hosting.WebRootPath, "Uploads");

            string oldfile = _context.Galleries.Find(id).ImageURL;
            string pathh = Path.Combine(uploadfolderr, oldfile);
            System.IO.File.Delete(pathh);


            var productimagee = _context.Galleries.Find(id);
            _context.Galleries.Remove(productimagee);
            _context.SaveChanges();

            return Ok();

        }



        [Route("/products/info")]

        public async Task<IActionResult> productdetailstest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var allProduct = await _unitOfWork.ProductsRepository.GetAllAsync();
            var getProductSameCategory = allProduct.Where(p=>p.CategoryId==product.CategoryId).ToList();

            var viewmodel = new ProductsViewModel
            {
                 Id=product.Id,
                 Name= product.Name,
                 Description= product.Description,
                 Brand= product.Brand,
                 DepartmentName=product.Department.Name,
                 CategoryName=product.Category.Name,
                 Price=product.Price,
                 Specification=product.Specification,
                 ProductGallery=product.Gallery,
                 ProductAlot= getProductSameCategory,
                 ProductImage=product.ProductImage
            };



            return View(viewmodel);
        }





    }
}
