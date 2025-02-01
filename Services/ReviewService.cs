using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly UserService _userService;

        public ReviewService(ReviewRepository reviewRepository, UserService userService)
        {
            _reviewRepository = reviewRepository;
            _userService = userService;
        }

        public void AddReview(Review review)
        {
            var user = _userService.GetUserById(review.UserId);
            if (user != null)
            {
                review.UserFullName = $"{user.FirstName} {user.LastName}".Trim();
            }
            else
            {
                review.UserFullName = "Unknown User";
            }

            review.DateAdded = DateTime.UtcNow;
            _reviewRepository.AddReview(review);
        }

        public List<Review> GetReviewsByProductId(string productId)
        {
            return string.IsNullOrEmpty(productId)
                ? new List<Review>()
                : _reviewRepository.GetReviewsByProductId(productId);
        }
    }
}
