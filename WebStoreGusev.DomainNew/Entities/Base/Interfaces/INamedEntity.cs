namespace WebStoreGusev.DomainNew.Entities.Base.Interfaces
{
    /// <summary>
    /// Сущность с Name.
    /// </summary>
    public interface INamedEntity : IBaseEntity
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        string Name { get; set; }
    }
}
