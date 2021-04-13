﻿using Intex_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                    var message = new System.Net.Mail.MailMessage();
                    message.To.Add(email);
                    message.Subject = "Verify Account";
                    message.From = new MailAddress("teo1997@byu.edu");

                    var emailDomain = model.Email.Substring(model.Email.LastIndexOf('@'));

                    var orgName = "@byu.edu" == emailDomain;
                    if (!orgName){
                        //you can't sign up
                        message.Body = "Sorry this is for @byu.edu email addresses only";
                    }
                    else
                    {
                        message.Body = email + ",\n" + System.Environment.NewLine + "Thank you for registering with Gamous. Click link to verify" + "https://localhost:44303/account/verify/" + user.Id;
                    }
                   



                    message.IsBodyHtml = true;
                    var smtp = new SmtpClient("smtp.outlook.com", 587)
                    {
                        Credentials = new NetworkCredential("teo1997@byu.edu", ""),
                        EnableSsl = true
                    };
                    smtp.Send(message);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Token", "token");
                }
                //}
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("verify/{userId}")]
        public async Task<IActionResult> VerifyAccount(String userId)
        {
            var user = _userManager.Users.First(u => u.Id == userId);
            user.EmailConfirmed = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                //take them to the token page
                return RedirectToAction("Token", "token");
            }
            return RedirectToAction("Home", "Index");
        }
    }
}
