using Intex_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
   
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
            var email = user.Email;
            user.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                //if (_userManager.FindByNameAsync(user.UserName).IsCompletedSuccessfully == false)
                //{
                    //the user does not already exist
                var result = await _userManager.CreateAsync(user, model.Password);
                if (email != null)
                {

                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Token");
                }
                //}
            }
            return View();
        }

    }
}
