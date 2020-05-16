using WebStoreGusev.DomainNew.Entities.Base.Interfaces;

namespace WebStoreGusev.DomainNew.Entities.Base
{
    public class NamedEntity : INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
