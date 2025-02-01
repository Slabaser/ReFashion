using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Components
{
    public class ReviewsViewComponent : ViewComponent
    {
        private readonly ReviewService _reviewService;

        public ReviewsViewComponent(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IViewComponentResult Invoke(string productId)
        {
            var reviews = _reviewService.GetReviewsByProductId(productId);
            return View(reviews);
        }
    }
}
