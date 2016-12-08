using IDSurvey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AdminController( 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signManager, 
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signManager;
            _logger = loggerFactory.CreateLogger<AdminController>();
        }
        
        [HttpGet]
        public IActionResult Index(ManageMessageId? message)
        {
            ViewData["StatusMessage"] =
               message == ManageMessageId.ChangePasswordSuccess ? "User password has been changed."
               : message == ManageMessageId.SetPasswordSuccess ? "User password has been set."
               : message == ManageMessageId.SetTwoFactorSuccess ? "User two-factor authentication provider has been set."
               : message == ManageMessageId.Error ? "An error has occurred."
               : message == ManageMessageId.AddPhoneSuccess ? "User phone number was added."
               : message == ManageMessageId.RemovePhoneSuccess ? "User phone number was removed."
               : "";

            var users = _userManager.Users.OrderBy(u => u.NormalizedUserName).ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForceResetPassword(string userId, string newPassword)
        {
            //change user's password
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                else
                {

                }    
            }
            else
            {
                ViewData["Message"] = "Cannot find user.";
            }

            return View();
        }

        #region Helpers
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        #endregion

    }
}
