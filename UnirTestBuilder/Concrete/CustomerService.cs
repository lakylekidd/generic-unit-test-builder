using UnitTestBuilder.Mappers;
using UnitTestBuilder.Models;

namespace UnitTestBuilder.Concrete
{
    public class CustomerService : ServiceBase<CustomerDto, Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICommunicationService _communicationService;
        private readonly IIdentityGenerator _identityGenerator;

        public CustomerService(
            ICustomerRepository customerRepository, 
            ICommunicationService communicationService, 
            IIdentityGenerator identityGenerator)
            : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _communicationService = communicationService;
            _identityGenerator = identityGenerator;
        }

        public async Task<Customer> CreateAsync(CustomerCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var id = _identityGenerator.Generate();
            var aggregate = model.ToAggregate(id);
            await _customerRepository.CreateAsync(aggregate);
            return aggregate;
        }

        public async Task DeleteAsync(int id)
        {
            if (await _customerRepository.GetAsync(id) is not { } customer)
                throw new Exception($"The customer with id {id} was not found");
            _customerRepository.Delete(customer);
        }

        public async Task<ICollection<CustomerDto>> GetAllAsync()
        {
            if (await _customerRepository.GetAsync() is not { Count: > 0 } customers)
                throw new Exception("No customers found.");
            return customers.Select(x => x.ToDto()).ToList();
        }
    }
}