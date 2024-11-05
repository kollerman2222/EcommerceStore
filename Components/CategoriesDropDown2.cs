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
using static TawassolProject.Contants.Permissions;

namespace TawassolProject.Components
{
    public class CategoriesDropDown2:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesDropDown2(IUnitOfWork unitOfWork)
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
