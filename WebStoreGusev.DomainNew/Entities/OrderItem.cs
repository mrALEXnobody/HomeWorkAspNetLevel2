using WebStoreGusev.DomainNew.Entities.Base;

namespace WebStoreGusev.DomainNew.Entities
{
    public class OrderItem : BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }    // сгенерирует внешний ключ в БД
    }
}
