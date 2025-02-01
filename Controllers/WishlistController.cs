using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "User")]
    public class WishlistController : Controller
    {
        private readonly WishlistService _wishlistService;
        private readonly UserService _userService;

        public WishlistController(WishlistService wishlistService, UserService userService)
        {
            _wishlistService = wishlistService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var wishlist = _wishlistService.GetWishlistByUserIdWithDetails(userId);
            if (wishlist == null || wishlist.Items == null || !wishlist.Items.Any())
            {
                ViewBag.Message = "Your wishlist is empty.";
                return View(new Wishlist { Items = new List<WishlistItem>() });
            }
            var totalFavorites = wishlist?.Items?.Count ?? 0;

            ViewBag.TotalFavorites = totalFavorites;
            return View(wishlist);
        }

        [HttpPost]
        [Route("Wishlist/AddToWishlist")]
        [HttpPost]
        public IActionResult AddToWishlist([FromBody] WishlistItem item)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "You must be logged in to add items to your wishlist." });
            }

            // Gelen verileri kontrol et
            if (item == null || string.IsNullOrEmpty(item.ProductId) || string.IsNullOrEmpty(item.ProductName))
            {
                return Json(new { success = false, message = "Product information is missing." });
            }

            try
            {
                Console.WriteLine($"Adding product to wishlist: {item.ProductId}, {item.ProductName}, {item.ProductPrice}");
                _wishlistService.AddToWishlist(userId, item.ProductId, item.ProductName, item.ProductPrice);
                return Json(new { success = true, message = "Product added to wishlist!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product to wishlist: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        [HttpPost]
        public IActionResult RemoveFromWishlist([FromBody] WishlistItem item)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "You must be logged in to remove items from your wishlist." });
            }

            if (string.IsNullOrEmpty(item.ProductId))
            {
                return Json(new { success = false, message = "Product ID is missing." });
            }

            try
            {
                Console.WriteLine($"Removing product: UserId={userId}, ProductId={item.ProductId}");
                _wishlistService.RemoveFromWishlist(userId, item.ProductId);
                return Json(new { success = true, message = "Product removed from wishlist!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing product from wishlist: {ex.Message}");
                return Json(new { success = false, message = "Failed to remove product from wishlist." });
            }
        }

        public IActionResult TotalFavorites()
        {
            var totalFavorites = _wishlistService.GetTotalFavoritesPerUser();
            return View(totalFavorites);
        }

        [HttpGet]
        public IActionResult IsProductInWishlist(string productId)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, inWishlist = false });
            }

            var wishlist = _wishlistService.GetWishlistByUserId(userId);
            if (wishlist != null && wishlist.Items.Any(i => i.ProductId == productId))
            {
                return Json(new { success = true, inWishlist = true });
            }

            return Json(new { success = true, inWishlist = false });
        }

    }
}
