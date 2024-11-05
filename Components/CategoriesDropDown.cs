using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TawassolProject.Data;
using TawassolProject.Models;
using TawassolProject.UnitOfWork;
using TawassolProject.ViewModel;

namespace TawassolProject.Components
{
    // i used another categoriesdropdown viewcompenent instead of categoriesdropdown2 because categoriesdropdown default.cshtml has another classes in html

    public class CategoriesDropDown:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesDropDown(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {

            var model = new CategoriesViewModel()
            {
                Categorys = _unitOfWork.CategoryRepository.GetAllAsync().Result,
            };

            return View(model);

        }

    }
}
