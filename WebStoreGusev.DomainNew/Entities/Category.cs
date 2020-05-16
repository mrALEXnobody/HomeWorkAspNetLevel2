using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStoreGusev.DomainNew.Entities.Base;
using WebStoreGusev.DomainNew.Entities.Base.Interfaces;

namespace WebStoreGusev.DomainNew.Entities
{
    [Table("Categories")]
    public class Category : NamedEntity, IOrderedEntity
    {
        public int? ParentId { get; set; }
        public int Order { get; set; }

        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
