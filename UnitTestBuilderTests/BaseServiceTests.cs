using FluentAssertions;
using UnitTestBuilder.Core;
using UnitTestBuilderTests.Builders;

namespace UnitTestBuilderTests
{
    public abstract class BaseServiceTests<TService, TDto, TAggregate, TBuilder, TRepository> 
        where TService : class, IService<TDto>
        where TBuilder : BaseBuilder<TService, TDto, TAggregate, TRepository>
        where TAggregate : class, IAggregate
        where TRepository : class, IRepository<TAggregate>
    {
        protected readonly TBuilder Builder;

        protected BaseServiceTests()
        {
            Builder = (TBuilder)Activator.CreateInstance(typeof(TBuilder))!;
        }

        [Fact]
        public async Task GetAsync_should_throw_when_no_customer_found()
        {
            var service = Builder.Build().Create();

            var action = async () => await service.GetAsync(2);

            await action.Should().ThrowAsync<Exception>().WithMessage($"The {typeof(TAggregate).Name} with id {2} was not found");
        }

        [Fact]
        public async Task GetAsync_should_return_correctly_mapped_customer()
        {
            var service = Builder
                .WithAggregate(out var expected)
                .Build().Create();

            var resulting = await service.GetAsync(1);

            CompareResultingCustomerAndVerifyMappings(expected, resulting);
        }

        protected abstract void CompareResultingCustomerAndVerifyMappings(TAggregate expected, TDto resulted);
    }
}
