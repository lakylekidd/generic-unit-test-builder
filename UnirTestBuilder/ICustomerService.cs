using UnitTestBuilder.Core;
using UnitTestBuilder.Models;

namespace UnitTestBuilder
{
    public interface ICustomerService : IService<CustomerDto>
    {
        Task<Customer> CreateAsync(CustomerCreateModel model);
    }
}
