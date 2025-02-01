using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Services;

namespace ECommerceApp.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly CategoryService _categoryService;

        public CategoryMenuViewComponent(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var womenCategory = _categoryService.GetCategoryByName("Women");
            var menCategory = _categoryService.GetCategoryByName("Men");

            ViewBag.WomenSubcategories = womenCategory?.Subcategories ?? new List<string>();
            ViewBag.MenSubcategories = menCategory?.Subcategories ?? new List<string>();

            return View();
        }
    }
}
