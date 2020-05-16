using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BrandsViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _brands = GetBrands();
            return View(_brands);
        }

        private List<BrandViewModel> GetBrands()
        {
            var brands = _productService.GetBrands();
            return brands.Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Order = b.Order,
                ProductsCount = 50
            }).OrderBy(b => b.Order).ToList();
        }
    }
}