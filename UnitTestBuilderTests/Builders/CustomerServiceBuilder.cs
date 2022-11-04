using Moq;
using UnitTestBuilder;
using UnitTestBuilder.Concrete;
using UnitTestBuilder.Models;

namespace UnitTestBuilderTests.Builders
{
    public class CustomerServiceBuilder : BaseBuilder<CustomerService, CustomerDto, Customer, ICustomerRepository>
    {
        public Mock<ICommunicationService> CommunicationsServiceMock { get; } = new();
       

        
        public ICommunicationService CommunicationsService => CommunicationsServiceMock.Object;
       
        
        public override CustomerServiceBuilder Build()
        {
            Services = new object[] { Repository, CommunicationsService, IdentityGenerator };
            return this;
        }

        public CustomerServiceBuilder WithCustomers(out ICollection<Customer> customers, int count = 3)
        {
            customers = Enumerable.Range(0, count).Select(x =>
            {
                var customer = GenerateAggregate(x);
                RepositoryMock.Setup(s => s.GetAsync(x)).ReturnsAsync(customer);
                return customer;
            }).ToList();
            RepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(customers);
            return this;
        }

        public CustomerServiceBuilder WithCreateModel(out CustomerCreateModel model, 
            string firstName = default,
            string lastName = default,
            string email = default,
            DateTime dob = default)
        {
            model = new CustomerCreateModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dob
            };
            return this;
        }

        protected override Customer GenerateAggregate(int withId)
        {
            return new Customer
            {
                Id = withId,
                FirstName = Guid.NewGuid().ToString()[..9],
                LastName = Guid.NewGuid().ToString()[..9],
                Email = $"{Guid.NewGuid().ToString()[..3]}@email.com",
                DateOfBirth = GetRandomDate()
            };
        }

        private static DateTime GetRandomDate()
        {
            var randomTest = new Random();
            var startDate = DateTime.UtcNow.AddYears(-80);
            var timeSpan = DateTime.UtcNow.AddYears(-10) - startDate;
            var newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
            return startDate + newSpan;
        }
    }
}
