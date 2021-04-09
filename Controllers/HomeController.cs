using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Intex_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Intex_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //public Token _token { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)//, Token token)
        {
            _logger = logger;
            _userManager = userManager;
            //_token = token;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewBurialsPublic()
        {
            //return View();
            return View();
        }
        [Authorize]
        public IActionResult ViewBurialsResearchers()
        {
            //return View();
            return View();
        }
        [Authorize]
        public IActionResult ResearchersTools()
        {
            //return View();
            return View();
        }
        [Authorize]
        public IActionResult EnterFieldNotes()
        {
            //return View();
            return View();
        }
        [Authorize]
        public IActionResult ViewFieldNotes()
        {
            //return View();
            return View();
        }
        [Authorize]
        public IActionResult EnterData()
        {
            //return View();
            return View();
        }
        [Authorize]
        public IActionResult ViewData()
        {
            //return View();
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult TestForm()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TestForm(string test, string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
