using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;
using System.Security.Claims;
using ECommerceApp.Repositories;

namespace ECommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;
        private readonly ProductRepository _productRepository;
        private readonly ReviewService _reviewService;

        public ProductsController(ProductService productService, WishlistService wishlistService, CartService cartService, ReviewService reviewService)
        {
            _productService = productService;
            _wishlistService = wishlistService;
            _cartService = cartService;
            _reviewService = reviewService;
        }

        // Kategoriye göre ürün listeleme
        public IActionResult Index(string category)
        {
            var products = _productService.GetProductsByCategory(category);
            ViewBag.Category = category;
            return View("Products", products);
        }

        // Alt kategoriye göre ürün listeleme
        public IActionResult BySubcategory(string category, string subcategory)
        {
            var products = _productService.GetProductsBySubcategory(category, subcategory);
            ViewBag.Category = category;
            ViewBag.Subcategory = subcategory;
            return View("Products", products);
        }

        // Ürün detaylarını görüntüleme
        public IActionResult Detail(string id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var reviews = _reviewService.GetReviewsByProductId(id); // Ürün yorumlarını al
            ViewBag.Reviews = reviews; // Yorumları View'a gönder

            return View(product);
        }

        // Arama sonuçları
        public IActionResult SearchResults(string query, string category)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<ProductDetail>());
            }

            var results = _productService.SearchProducts(query, category);
            ViewBag.Query = query;
            ViewBag.Category = category;
            return View(results);
        }
        //wishlist
        public IActionResult AddToWishlist(string productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "You must be logged in to add items to your wishlist." });
            }

            var userId = HttpContext.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User session is not valid. Please log in again." });
            }

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            try
            {
                _wishlistService.AddToWishlist(userId, product.Id, product.Name, product.Price);
                return Json(new { success = true, message = "Product added to wishlist!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddToWishlist: {ex.Message}");
                return Json(new { success = false, message = $"Failed to add product to wishlist: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult AddToCart(string productId, string productSize, int quantity)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "You must be logged in to add items to your cart." });
            }

            try
            {
                _cartService.AddToCart(userId, productId, productSize, quantity);
                return Json(new { success = true, message = "Product added to cart!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateStock(string productId, int quantity)
        {
            try
            {
                var product = _productRepository.GetProductById(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found." });
                }

                if (product.StockCount < quantity)
                {
                    return Json(new { success = false, message = "Insufficient stock." });
                }

                product.StockCount -= quantity;
                _productRepository.UpdateProductStock(product);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult NewArrivals()
        {
            try
            {
                // Son 10 gün içinde eklenen ürünleri al
                var newArrivals = _productService.GetNewArrivals(10); // 10 gün varsayılan olarak ayarlandı

                if (newArrivals == null || !newArrivals.Any())
                {
                    // Eğer ürün yoksa bilgilendirici bir mesaj döndür
                    ViewBag.Message = "No new arrivals in the last 10 days.";
                    return View("Products", new List<ProductDetail>());
                }

                // Yeni ürünleri Products view'e gönder
                return View("Products", newArrivals);
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama ve bilgilendirme
                Console.WriteLine($"Error fetching new arrivals: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while fetching new arrivals.";
                return View("Products", new List<ProductDetail>());
            }
        }

        [HttpGet]
        public IActionResult Bestseller()
        {
            // Tüm sepetleri getir
            var allCarts = _cartService.GetAllCarts();

            // Ürünlerin satış miktarını hesapla
            var bestsellers = allCarts
                .SelectMany(cart => cart.Items)
                .GroupBy(item => item.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalQuantity = group.Sum(item => item.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(10) // En çok satan ilk 10 ürünü getir
                .ToList();

            // Ürün detaylarını getir
            var bestsellerProducts = bestsellers
                .Select(b => _productService.GetProductById(b.ProductId))
                .ToList();

            return View("Products", bestsellerProducts); // Products.cshtml view'ini kullan
        }




    }



}

