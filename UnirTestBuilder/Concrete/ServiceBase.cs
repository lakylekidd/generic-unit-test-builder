using UnitTestBuilder.Core;
using UnitTestBuilder.Mappers;
using UnitTestBuilder.Models;

namespace UnitTestBuilder.Concrete
{
    public abstract class ServiceBase<TDto, TAggregate> : IService<TDto>
        where TAggregate : class, IAggregate
        where TDto : BaseDto
    {
        protected readonly IIdentityGenerator IdentityGenerator;
        protected readonly IRepository<TAggregate> Repository;

        protected ServiceBase(IIdentityGenerator identityGenerator, IRepository<TAggregate> repository)
        {
            IdentityGenerator = identityGenerator;
            Repository = repository;
        }

        public async Task<TDto> GetAsync(int id)
        {
            if (await Repository.GetAsync(id) is not { } customer)
                throw new Exception($"The {typeof(TAggregate).Name} with id {id} was not found");
            return (customer.ToDerivedDto() as TDto)!;
        }
    }
}
