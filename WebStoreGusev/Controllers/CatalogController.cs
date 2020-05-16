using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStoreGusev.DomainNew;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;

        public CatalogController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Shop(int? categoryId, int? brandId)
        {
            var products = _productService.GetProducts(
                new ProductFilter { BrandId = brandId, CategoryId = categoryId });

            var model = new CatalogViewModel()
            {
                BrandId = brandId,
                CategoryId = categoryId,
                Products = products.Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
                    Brand = p.Brand?.Name ?? string.Empty
                }).OrderBy(p => p.Order)
                    .ToList()
            };

            return View(model);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
                return NotFound();

            return View(new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Brand = product.Brand != null ? product.Brand.Name : string.Empty
            });
        }
    }
}