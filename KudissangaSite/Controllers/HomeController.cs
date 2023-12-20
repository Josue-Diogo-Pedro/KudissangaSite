using KudissangaSite.Areas.Identity.Data;
using KudissangaSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KudissangaSite.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly KudissangaSiteContext _context;

        public HomeController(KudissangaSiteContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Suites
        public async Task<IActionResult> Index()
        {
            var context1 = _context;
            var id = context1.UserClaims.FirstOrDefault();
            return View(await _context.Suites.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Suites
        public async Task<IActionResult> QuemSomos()
        {
            var context1 = _context;
            var id = context1.UserClaims.FirstOrDefault();
            return View(await _context.Suites.ToListAsync());
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