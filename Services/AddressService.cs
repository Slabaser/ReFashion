using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class AddressService
    {
        private readonly AddressRepository _addressRepository;

        public AddressService(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public void AddAddress(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            }

            if (string.IsNullOrEmpty(address.FullName) || string.IsNullOrEmpty(address.Street))
            {
                throw new ArgumentException("Address details are incomplete.");
            }

            _addressRepository.AddAddress(address);
        }

        public Address GetAddressById(string id)
        {


            return _addressRepository.GetAddressById(id);
        }
    }
}
