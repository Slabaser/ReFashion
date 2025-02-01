using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class ShippingService
    {
        private readonly ShippingRepository _shippingRepository;

        public ShippingService(ShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public void AddShipping(Shipping shipping)
        {
            if (string.IsNullOrEmpty(shipping.UserId))
            {
                throw new ArgumentException("UserId is required.");
            }

            _shippingRepository.AddShipping(shipping);
        }

        public Shipping GetShippingById(string id)
        {
            return _shippingRepository.GetShippingById(id);
        }

        public List<Shipping> GetShippingsByUserId(string userId)
        {
            return _shippingRepository.GetShippingsByUserId(userId);
        }
    }
}
