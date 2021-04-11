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
using Microsoft.EntityFrameworkCore;
using Intex_app.Models.ViewModels;

namespace Intex_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //public Token _token { get; set; }
        private GamousContext _context { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, GamousContext context)//, Token token)
        {
            _logger = logger;
            _userManager = userManager;
            //_token = token;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult arcgis()
        {
            return View();
        }



        //public IActionResult ViewBurialsPublic(long? id)
        //{
        //    //return View();
        //    return View(_context.LocationMeasurements
        //        .FromSqlInterpolated($"SELECT * FROM LocationMeasurement WHERE Id = {id} or {id} IS NULL")
        //        .ToList());

        //    //return View(_context.LocationMeasurements);
        //}

        #region Public View
        public IActionResult ViewBurialsPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                LocationMeasurements = (_context.LocationMeasurements
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.LocationMeasurements.Count() :
                     _context.LocationMeasurements.Where(x => x.Id == id).Count())
                },

                Osteologies = (_context.Osteologies
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList())


            });
        }
        public IActionResult OsteologyPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                Osteologies = (_context.Osteologies
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Osteologies.Count() :
                     _context.Osteologies.Where(x => x.Id == id).Count())
                },


            });
        }
        public IActionResult OsteologySkullPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                OsteologySkulls = (_context.OsteologySkulls
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.OsteologySkulls.Count() :
                     _context.OsteologySkulls.Where(x => x.Id == id).Count())
                },


            });
        }

        public IActionResult DemographicPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                Demographics = (_context.Demographics
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Demographics.Count() :
                     _context.Demographics.Where(x => x.Id == id).Count())
                },


            });
        }

        public IActionResult ArtifactBioNotePublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                ArtifactBioNotes = (_context.ArtifactBioNotes
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.ArtifactBioNotes.Count() :
                     _context.ArtifactBioNotes.Where(x => x.Id == id).Count())
                },


            });
        }
        #endregion



        [Authorize]
        public IActionResult ViewBurialsResearchers(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                LocationMeasurementsR = (_context.LocationMeasurements
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.LocationMeasurements.Count() :
                     _context.LocationMeasurements.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult OsteologyR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                OsteologiesR = (_context.Osteologies
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Osteologies.Count() :
                     _context.Osteologies.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult OsteologySkullR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                OsteologySkullsR = (_context.OsteologySkulls
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.OsteologySkulls.Count() :
                     _context.OsteologySkulls.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult DemographicR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                DemographicsR = (_context.Demographics
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Demographics.Count() :
                     _context.Demographics.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult ArtifactBioNoteR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                ArtifactBioNotesR = (_context.ArtifactBioNotes
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.ArtifactBioNotes.Count() :
                     _context.ArtifactBioNotes.Where(x => x.Id == id).Count())
                },

            });
        }

        [HttpGet]
        public IActionResult CRUD_AddData()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CRUD_AddData(LocationMeasurement newdata)
        {
            if (ModelState.IsValid)
            {
                _context.LocationMeasurements.Add(newdata);
                _context.SaveChanges();

                return RedirectToAction("ViewBurialsResearchers");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CRUD_UpdateData(string id)
        {
            LocationMeasurement data = _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult CRUD_UpdateData(LocationMeasurement data, string id)
        {
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Nors = data.Nors;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HighNs = data.HighNs;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().LowNs = data.LowNs;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Eorw = data.Eorw;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HighEw = data.HighEw;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().LowEw = data.LowEw;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Square = data.Square;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().BurialNum = data.BurialNum;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Direction = data.Direction;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Depth = data.Depth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HeadSouth = data.HeadSouth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().FeetSouth = data.FeetSouth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HeadWest = data.HeadWest;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().FeetWest = data.FeetWest;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().BurialLength = data.BurialLength;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().DiscoveryDay = data.DiscoveryDay;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().DiscoveryMonth = data.DiscoveryMonth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().DiscoveryYear = data.DiscoveryYear;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Cluster = data.Cluster;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().BurialRack = data.BurialRack;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().CreatedBy = data.CreatedBy;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().LastModifiedBy = data.LastModifiedBy;

            _context.SaveChanges();

            return RedirectToAction("ViewBurialsResearchers");
        }

        [HttpPost]
        public IActionResult CRUD_DeleteData(string id)
        {
            var data = _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault();
            _context.LocationMeasurements.Remove(data);
            _context.SaveChangesAsync();

            return RedirectToAction("ViewBurialsResearchers");
        }

        [HttpGet]
        public IActionResult CRUD_AddNote()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CRUD_AddNote(ArtifactBioNote newdata)
        {
            if (ModelState.IsValid)
            {
                _context.ArtifactBioNotes.Add(newdata);
                _context.SaveChanges();

                return RedirectToAction("ArtifactBioNoteR");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CRUD_UpdateNote(string id)
        {
            ArtifactBioNote data = _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult CRUD_UpdateNote(ArtifactBioNote data, string id)
        {
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().Rack = data.Rack;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().ArtifactFound = data.ArtifactFound;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().ArtifactDescription = data.ArtifactDescription;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().SampleTaken = data.SampleTaken;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().BioNotes = data.BioNotes;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().AdditionalNotes = data.AdditionalNotes;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().FaceBundle = data.FaceBundle;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().PathologyAnomalies = data.PathologyAnomalies;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().BurialWraping = data.BurialWraping;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().PreservationIndex = data.PreservationIndex;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().CreatedBy = data.CreatedBy;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().LastModifiedBy = data.LastModifiedBy;

            _context.SaveChanges();

            return RedirectToAction("ArtifactBioNoteR");
        }

        [HttpPost]
        public IActionResult CRUD_DeleteNote(string id)
        {
            var data = _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault();
            _context.ArtifactBioNotes.Remove(data);
            _context.SaveChangesAsync();

            return RedirectToAction("ArtifactBioNoteR");
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
