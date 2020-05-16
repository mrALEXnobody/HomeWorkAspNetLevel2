using System.Collections.Generic;
using WebStoreGusev.DomainNew;
using WebStoreGusev.DomainNew.Entities;

namespace WebStoreGusev.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Brand> GetBrands();
        IEnumerable<Product> GetProducts(ProductFilter filter);
        Product GetProductById(int id);
    }
}