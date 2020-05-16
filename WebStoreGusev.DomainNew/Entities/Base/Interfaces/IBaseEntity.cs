namespace WebStoreGusev.DomainNew.Entities.Base.Interfaces
{
    /// <summary>
    /// Базовая сущность с Id
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        int Id { get; set; }
    }
}
