using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;
using ECommerceApp.Services;

namespace ECommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, CategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            try
            {
                var womenCategory = _categoryService.GetCategoryByName("Women");
                var menCategory = _categoryService.GetCategoryByName("Men");

                ViewBag.WomenSubcategories = womenCategory?.Subcategories ?? new List<string>();
                ViewBag.MenSubcategories = menCategory?.Subcategories ?? new List<string>();

                var isAuthenticated = HttpContext.Session.GetString("UserId") != null;
                ViewBag.IsAuthenticated = isAuthenticated;

                ViewData["Title"] = "ReFashion";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading homepage.");
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
