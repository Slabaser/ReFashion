namespace ECommerceApp.Models
{
    public class ConfirmationViewModel
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Shipping Details
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }

        // Order Items
        public List<CartItem> Items { get; set; }
    }
}
