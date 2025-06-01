using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceApp.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IMongoDatabase _database;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _cartService.GetCartByUserIdWithDetails(userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                ViewBag.Message = "Your cart is empty. Start shopping now!";
                return View(new List<CartItem>());
            }

            return View(cart.Items);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemDto cartItem)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "You must be logged in to add items to your cart." });
            }

            try
            {
                _cartService.AddToCart(userId, cartItem.ProductId, cartItem.ProductSize, cartItem.Quantity);
                return Json(new { success = true, message = "Product added to cart!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCartByProductId([FromBody] string productId)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "You must be logged in to remove items from your cart." });
            }

            try
            {
                _cartService.RemoveFromCartByProductId(userId, productId);
                return Json(new { success = true, message = "Product removed from cart." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] CartItemDto itemDto)
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            try
            {
                if (itemDto.Quantity > 10)
                {
                    return Json(new { success = false, message = "Maximum quantity allowed is 10." });
                }

                _cartService.UpdateCartItemQuantity(userId, itemDto.ProductId, itemDto.Quantity);
                return Json(new { success = true, message = "Quantity updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            try
            {
                _cartService.ClearCart(userId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult CalculateTotal(IEnumerable<CartItem> cartItems, decimal discount)
        {
            try
            {
                var result = _database.RunCommand<BsonDocument>(new BsonDocument
                {
                    { "eval", "calculateGrandTotal" },
                    { "args", new BsonArray { cartItems.ToBsonDocument(), discount } }
                });

                var grandTotal = result["retval"].AsDecimal;
                return View(grandTotal);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error calculating total: " + ex.Message);
                return View(0);
            }
        }

        public class CartItemDto
        {
            public string ProductId { get; set; }
            public string ProductSize { get; set; }
            public int Quantity { get; set; }
        }

        public class CartItemUpdateDto
        {
            public string CartItemId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
