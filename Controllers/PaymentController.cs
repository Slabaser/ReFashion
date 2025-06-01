using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "User")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly CartService _cartService;

        public PaymentController(PaymentService paymentService, CartService cartService)
        {
            _paymentService = paymentService;
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _cartService.GetCartByUserIdWithDetails(userId);

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var subtotal = cart.Items.Sum(item => item.ProductPrice * item.Quantity);
            var grandTotal = subtotal + 7.5m;

            var paymentViewModel = new PaymentViewModel
            {
                FullName = HttpContext.User.FindFirst("FullName")?.Value,
                CartItems = cart.Items,
                Subtotal = subtotal,
                ShippingCost = 7.5m,
                GrandTotal = grandTotal
            };

            return View(paymentViewModel);
        }

        [HttpPost]
        public IActionResult Index(PaymentViewModel model)
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
                _paymentService.CompletePayment(userId, model);
                return RedirectToAction("OrderConfirmation", "Checkout");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult ProcessPayment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var userId = HttpContext.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cart = _cartService.GetCartByUserIdWithDetails(userId);

                if (cart == null || !cart.Items.Any())
                {
                    ModelState.AddModelError("", "Your cart is empty. Please add items to your cart before proceeding.");
                    return View("Index", model);
                }

                var subtotal = cart.Items.Sum(item => item.ProductPrice * item.Quantity);
                var grandTotal = subtotal + model.ShippingCost;

                _paymentService.CompletePayment(userId, new PaymentViewModel
                {
                    Subtotal = subtotal,
                    ShippingCost = model.ShippingCost,
                    GrandTotal = grandTotal,
                    CartItems = cart.Items,
                    FullName = model.FullName,
                    PaymentMethod = model.PaymentMethod,
                    NameOnCard = model.NameOnCard,
                    CardNumber = model.CardNumber,
                    ExpirationDate = model.ExpirationDate,
                    CVV = model.CVV
                });

                return RedirectToAction("OrderConfirmation", "Checkout");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", model);
            }
        }
    }
}
