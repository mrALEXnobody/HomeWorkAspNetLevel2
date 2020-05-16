using System.Collections.Generic;

namespace WebStoreGusev.Models
{
    public class CatalogViewModel
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
