namespace UnitTestBuilder.Core
{
    public interface IService<TDto>
    {
        Task<TDto> GetAsync(int id);
    }
}
