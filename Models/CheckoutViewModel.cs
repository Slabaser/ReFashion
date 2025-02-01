namespace ECommerceApp.Models
{
    public class CheckoutViewModel
    {
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; } // Kargo maliyeti

        // Kullanıcı bilgileri
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

    }

}