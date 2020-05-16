using System.Collections.Generic;
using System.Linq;

namespace WebStoreGusev.Models
{
    public class CartViewModel
    {
        public Dictionary<ProductViewModel, int> Items { get; set; }
        public int ItemsCount => Items?.Sum(x => x.Value) ?? 0;
    }
}
