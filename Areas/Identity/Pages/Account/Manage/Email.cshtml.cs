using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TawassolProject.Models;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace TawassolProject.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

            [EmailAddress]
            [Display(Name = "Current email")]
            public string CurrentEmail { get; set; }
        }

    
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {
              CurrentEmail = user.Email,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(OnGetAsync));
            }

            var searchRandomUserWithEmail = await _userManager.FindByEmailAsync(Input.NewEmail);
            if (searchRandomUserWithEmail != null && searchRandomUserWithEmail.Id != user.Id)
            {
                StatusMessage = "Error Email is already taken";

            }
            else if (Input.NewEmail != null && Input.NewEmail == user.Email)
            {
                StatusMessage = "Your email is unchanged";
            }
            else if (Input.NewEmail != null && Input.NewEmail != user.Email)
            {
                user.Email = Input.NewEmail;
                user.UserName = new MailAddress(Input.NewEmail).User;
                await _userManager.UpdateAsync(user);
                StatusMessage = "Email changed successfully";
            }

            return RedirectToPage();
        }


    }
}
