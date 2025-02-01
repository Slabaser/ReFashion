using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;
using ECommerceApp.Services;

namespace ECommerceApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CategoryService _categoryService;

    // Constructor düzenlendi
    public HomeController(ILogger<HomeController> logger, CategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        var womenCategory = _categoryService.GetCategoryByName("Women");
        var menCategory = _categoryService.GetCategoryByName("Men");

        // Alt kategorileri ViewBag'e atıyoruz
        ViewBag.WomenSubcategories = womenCategory?.Subcategories ?? new List<string>();
        ViewBag.MenSubcategories = menCategory?.Subcategories ?? new List<string>();

        var isAuthenticated = HttpContext.Session.GetString("UserId") != null;
        ViewBag.IsAuthenticated = isAuthenticated;

        ViewData["Title"] = "ReFashion";
        return View();

        
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
