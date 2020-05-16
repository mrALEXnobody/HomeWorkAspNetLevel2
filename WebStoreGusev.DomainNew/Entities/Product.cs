using System.ComponentModel.DataAnnotations.Schema;
using WebStoreGusev.DomainNew.Entities.Base;
using WebStoreGusev.DomainNew.Entities.Base.Interfaces;

namespace WebStoreGusev.DomainNew.Entities
{
    [Table("Products")]
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        /// <summary>
        /// Размер. Поле для ДЗ.
        /// </summary>
        public int Size { get; set; }
    }
}
