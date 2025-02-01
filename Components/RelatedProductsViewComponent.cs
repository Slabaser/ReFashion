using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Components
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly ProductService _productService;

        public RelatedProductsViewComponent(ProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(string category, string subcategory, string currentProductId)
        {
            // Aynı kategori ve alt kategoriye sahip, ancak görüntülenen üründen farklı ürünleri getir
            var relatedProducts = _productService.GetProductsByCategoryAndSubcategoryWithoutCurrent(category, subcategory, currentProductId);
            return View(relatedProducts);
        }
    }
}
