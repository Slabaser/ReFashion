using System.Collections.Generic;

namespace ECommerceApp.Models
{
    public class ProductDetailViewModel
    {
        public ProductDetail Product { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
