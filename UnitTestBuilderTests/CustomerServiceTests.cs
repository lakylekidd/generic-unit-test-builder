using FluentAssertions;
using Moq;
using UnitTestBuilder;
using UnitTestBuilder.Concrete;
using UnitTestBuilder.Models;
using UnitTestBuilderTests.Builders;

namespace UnitTestBuilderTests
{
    public class CustomerServiceTests : BaseServiceTests<CustomerService, CustomerDto, Customer, CustomerServiceBuilder, ICustomerRepository>
    {
        [Fact]
        public async Task CreateAsync_should_throw_when_no_model_provided()
        {
            var service = Builder.Build().Create();

            var action = async () => await service.CreateAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAsync_should_throw_when_id_could_not_be_generated()
        {
            var service = Builder
                .WithCreateModel(out var model)
                .WithIdGeneratorFailure()
                .Build().Create();

            var action = async () => await service.CreateAsync(model);

            await action.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task CreateAsync_should_throw_when_id_could_not_be_generated_specific_exception()
        {
            var service = Builder
                .WithCreateModel(out var model)
                .WithIdGeneratorFailure(new AccessViolationException("This is a custom message"))
                .Build().Create();

            var action = async () => await service.CreateAsync(model);

            await action.Should().ThrowAsync<AccessViolationException>().WithMessage("This is a custom message");
        }

        [Fact]
        public async Task CreateAsync_should_create_successfully()
        {
            var service = Builder
                .WithCreateModel(out var model, firstName: "Tester")
                .WithId()
                .Build().Create();

            var resultingAggregate = await service.CreateAsync(model);

            resultingAggregate.Id.Should().Be(1);
            resultingAggregate.FirstName.Should().Be("Tester");
        }

        [Fact]
        public async Task DeleteAsync_should_throw_when_customer_not_found()
        {
            var service = Builder
                .WithAggregate(out _)
                .Build().Create();

            var action = async () => await service.DeleteAsync(2);

            await action.Should().ThrowAsync<Exception>()
                .WithMessage($"The customer with id {2} was not found");
        }

        [Fact]
        public async Task DeleteAsync_should_delete_customer()
        {
            var service = Builder
                .WithAggregate(out var customer)
                .Build().Create();

            await service.DeleteAsync(1);

            Builder.RepositoryMock.Verify(x => x.Delete(customer), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_should_throw_when_no_customers_found()
        {
            var service = Builder.Build().Create();

            var action = async () => await service.GetAllAsync();

            await action.Should().ThrowAsync<Exception>().WithMessage("No customers found.");
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetAllAsync_should_return_correctly_mapped_customers(int customerCount)
        {
            var service = Builder
                .WithCustomers(out var expectedCustomers, customerCount)
                .Build().Create();

            var resultingCustomers = await service.GetAllAsync();

            resultingCustomers.Should().HaveCount(expectedCustomers.Count);
            foreach (var customer in expectedCustomers)
                CompareResultingCustomerAndVerifyMappings(customer, resultingCustomers.FirstOrDefault(y => y.Id == customer.Id)!);
        }

        protected override void CompareResultingCustomerAndVerifyMappings(Customer expected, CustomerDto resulted)
        {
            resulted.Should().NotBeNull();
            resulted!.Email.Should().Be(expected.Email);
            resulted!.FullName.Should().Be($"{expected.FirstName} {expected.LastName}");
            resulted!.Age.Should().Be(DateTime.UtcNow.Year - expected.DateOfBirth.Year);
        }
    }
}
