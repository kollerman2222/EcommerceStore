using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public DepartmentsController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var deparments =  await _unitOfWork.DepartmentsRepository.GetAllAsync();
            var dep = deparments.Select(d => new DepartmentsViewModel
            {
                Id= d.Id,
                Name = d.Name,
                DepartmentImage = d.DepartmentImage,
            });
            return View(dep);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.DepartmentsRepository.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            var dep = new DepartmentsViewModel
            {
                Name= department.Name,
                Id= department.Id,
                DepartmentImage= department.DepartmentImage,
                Categorys=department.Categorys,
                Productss=department.Productss,
            };

            return View(dep);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentsViewModel dvm)
        {
                if (!ModelState.IsValid)
                {
                    return View("Create");
                }
               
                await _unitOfWork.DepartmentsRepository.CreateAsync(dvm);
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

            var department = await _unitOfWork.DepartmentsRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewmodel = new DepartmentsViewModel
            {
               Name=department.Name,
               Id=department.Id,
               DepartmentImage=department.DepartmentImage

            };

            return View(viewmodel);
        }

              
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentsViewModel dvm)
        {
            if (dvm.Id == null)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View("Edit");
            }

            var department = await _unitOfWork.DepartmentsRepository.GetByIdAsync(dvm.Id);

            if (department == null)
            {
                return NotFound();
            }

            string oldDepartmentImage = department.DepartmentImage;

            department.Name=dvm.Name;

               
            if (dvm.SingleImageUpload != null)
            {
                department.DepartmentImage= await _unitOfWork.DepartmentsRepository.SingleImageUploadAsync(dvm.SingleImageUpload);
                var oldImageDelete = Path.Combine(_unitOfWork.DepartmentsRepository.uploadFolderPublic, oldDepartmentImage);
                System.IO.File.Delete(oldImageDelete);
            }




                _unitOfWork.SaveChanges();

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

            var department = await _unitOfWork.DepartmentsRepository.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            _unitOfWork.DepartmentsRepository.Delete(department);
            _unitOfWork.SaveChanges();

            return View("Index");
        }

        

      
    }
}
