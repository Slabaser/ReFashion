using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "User")]
    public class CheckoutController : Controller
    {
        private readonly CheckoutService _checkoutService;
        private readonly CartService _cartService;
        private readonly AddressService _addresService;

        public CheckoutController(CheckoutService checkoutService, CartService cartService, AddressService addressService)
        {
            _checkoutService = checkoutService;
            _cartService = cartService;
            _addresService = addressService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var userId = HttpContext.User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = _cartService.GetCartByUserIdWithDetails(userId);

                if (cart == null || cart.Items == null || !cart.Items.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                var subtotal = cart.Items.Sum(item => item.ProductPrice * item.Quantity);

                var model = new CheckoutViewModel
                {
                    Subtotal = subtotal,
                    Discount = 0,
                    GrandTotal = subtotal + 7.5m,
                    ShippingMethod = "Standard Shipping"
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while preparing checkout: " + ex.Message);
                return View(new CheckoutViewModel());
            }
        }

        [HttpPost]
        public IActionResult Index(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                _checkoutService.ProcessOrder(userId, model);
                TempData["CheckoutData"] = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                return RedirectToAction("Index", "Payment");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult OrderConfirmation()
        {
            try
            {
                var userId = HttpContext.User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var order = _checkoutService.GetLastOrder(userId);

                if (order == null)
                {
                    return RedirectToAction("Index", "Cart");
                }

                var model = new ConfirmationViewModel
                {
                    OrderId = order.Id,
                    Items = order.Items,
                    TotalAmount = order.TotalAmount,
                    OrderDate = order.OrderDate
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching order confirmation: " + ex.Message);
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
