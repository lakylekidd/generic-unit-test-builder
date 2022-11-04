using UnitTestBuilder.Models;

namespace UnitTestBuilder.Mappers
{
    public static class CustomerMapper
    {
        public static Customer ToAggregate(this CustomerCreateModel m, int id) => new()
        {
            Id = id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            Email = m.Email,
            DateOfBirth = m.DateOfBirth
        };

        public static CustomerDto ToDto(this Customer c) => new()
        {
            Id = c.Id,
            FullName = $"{c.FirstName} {c.LastName}",
            Email = c.Email,
            Age = DateTime.UtcNow.Year - c.DateOfBirth.Year
        };
    }
}
