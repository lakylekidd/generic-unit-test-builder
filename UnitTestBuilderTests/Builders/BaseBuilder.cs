using Moq;
using UnitTestBuilder;
using UnitTestBuilder.Core;

namespace UnitTestBuilderTests.Builders
{
    public abstract class BaseBuilder<TService, TDto, TAggregate, TRepository>
        where TService : class, IService<TDto> 
        where TAggregate : class, IAggregate
        where TRepository : class, IRepository<TAggregate>
    {
        protected object[] Services = { };

        public Mock<IIdentityGenerator> IdentityGeneratorMock { get; } = new();
        public Mock<TRepository> RepositoryMock { get; } = new();

        public TRepository Repository => RepositoryMock.Object;
        public IIdentityGenerator IdentityGenerator => IdentityGeneratorMock.Object;

        public TService Create()
        {
            return ((TService)Activator.CreateInstance(typeof(TService), Services)!)!;
        }

        public BaseBuilder<TService, TDto, TAggregate, TRepository> WithAggregate(out TAggregate aggregate,
            int withId = 1)
        {
            aggregate = GenerateAggregate(withId);
            RepositoryMock.Setup(x => x.GetAsync(withId)).ReturnsAsync(aggregate);
            return this;
        }

        public BaseBuilder<TService, TDto, TAggregate, TRepository> WithIdGeneratorFailure(Exception e = default)
        {
            IdentityGeneratorMock.Setup(x => x.Generate()).Throws(e);
            return this;
        }

        public BaseBuilder<TService, TDto, TAggregate, TRepository> WithId(int id = 1)
        {
            IdentityGeneratorMock.Setup(x => x.Generate()).Returns(id);
            return this;
        }

        public abstract BaseBuilder<TService, TDto, TAggregate, TRepository> Build();
        protected abstract TAggregate GenerateAggregate(int withId);
    }
}
