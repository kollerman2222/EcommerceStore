using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TawassolProject.Data;
using TawassolProject.Models;
using TawassolProject.UnitOfWork;
using TawassolProject.ViewModel;

namespace TawassolProject.Controllers
{

    [AllowAnonymous]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var categorylist = await _unitOfWork.CategoryRepository.GetAllAsync();
            var cat = categorylist.Select(c => new CategoriesViewModel
            {
                Id = c.Id,
                Name = c.Name,
                DepartmentName=c.Department.Name
            });
            return View(cat);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var cat = new CategoriesViewModel
            {
                Id=category.Id,
                Name= category.Name,
                DepartmentName=category.Department.Name,
                Productss=category.Productss,
            };


            return View(cat);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.DepartmentId = new SelectList(await _unitOfWork.DepartmentsRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriesViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            await _unitOfWork.CategoryRepository.CreateAsync(cvm);
            _unitOfWork.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            var cat = new CategoriesViewModel
            {
                Id = category.Id,
                Name = category.Name,
                DepartmentName = category.Department.Name,
                Productss = category.Productss,
            };


            ViewBag.DepartmentId = new SelectList(await _unitOfWork.DepartmentsRepository.GetAllAsync(), "Id", "Name", category.DepartmentId);
            return View(cat);
        }

        // POST: Categories/Edit/5           
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoriesViewModel cvm)
        {


            if (ModelState.IsValid)
            {
                return View("Edit");
            }

            if(cvm.Id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(cvm.Id);
            if(category == null)
            {
                return BadRequest();
            }

            string oldCategoryImage = category.CategoryImage;

            category.Name = cvm.Name;
            category.DepartmentId=cvm.DepartmentId;
            if(cvm.SingleImageUpload != null)
            {
                category.CategoryImage = await _unitOfWork.CategoryRepository.SingleImageUploadAsync(cvm.SingleImageUpload);
                var oldImageDelete = Path.Combine(_unitOfWork.CategoryRepository.uploadFolderPublic, oldCategoryImage);
                System.IO.File.Delete(oldImageDelete);
            }
                
                                    
             return RedirectToAction(nameof(Index));
            
           
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return BadRequest();
            }
            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.SaveChanges();

            return View(category);
        }

        

        
    }
}
