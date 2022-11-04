namespace UnitTestBuilder.Core
{
    public interface IRepository<TAggregate>
        where TAggregate : class, IAggregate
    {
        Task<TAggregate> GetAsync(int id);
        Task<ICollection<TAggregate>> GetAsync();
        Task CreateAsync(TAggregate aggregate);
        void Delete(TAggregate aggregate);
    }
}
