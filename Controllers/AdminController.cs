using ECommerceApp.Services;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ECommerceApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public AdminController(UserService userService, ProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                return RedirectToAction("Login");
            }

            var users = _userService.GetAllUsers();
            var products = _productService.GetAllProducts();

            ViewBag.TotalUsers = users.Count;
            ViewBag.TotalProducts = products.Count;

            return View();
        }

        public IActionResult Users()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                return RedirectToAction("Login");
            }

            var users = _userService.GetAllUsers();
            return View(users);
        }

        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                return RedirectToAction("Login");
            }

            _userService.DeleteUser(id);
            return RedirectToAction("Users");
        }

        public IActionResult Products()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                return RedirectToAction("Login");
            }

            var products = _productService.GetAllProducts();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDetail product, IFormFile imageFile)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                return RedirectToAction("Login");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _productService.AddProduct(product);
            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult DeleteProduct(string id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                return RedirectToAction("Login");
            }

            _productService.DeleteProduct(id);
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("AdminUser", username);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUser");
            return RedirectToAction("Login");
        }
    }
}
