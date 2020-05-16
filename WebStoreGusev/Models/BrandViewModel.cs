using WebStoreGusev.DomainNew.Entities.Base.Interfaces;

namespace WebStoreGusev.Models
{
    public class BrandViewModel : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Количество товаров.
        /// </summary>
        public int ProductsCount { get; set; }
    }
}
