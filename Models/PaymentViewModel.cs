using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Name on card is required.")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        public string CVV { get; set; }

        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrandTotal { get; set; }
        public string ShippingMethod { get; set; }

        // Sepet Bilgisi (gerekirse ödeme ekranında gösterilir)
        public List<CartItem> CartItems { get; set; } // Sepetteki ürünlerin listesi
    }
}
