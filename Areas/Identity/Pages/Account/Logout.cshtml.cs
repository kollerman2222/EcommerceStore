using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TawassolProject.Models;
using TawassolProject.UnitOfWork;

namespace TawassolProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger , IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _unitOfWork.ShoppingCartRepository.ClearCart();
            _unitOfWork.SaveChanges();
            return RedirectToAction("HomePage", "Home");
        }
    }
}
