using UnitTestBuilder.Core;
using UnitTestBuilder.Models;

namespace UnitTestBuilder.Mappers
{
    public static class CommonMapper
    {
        public static BaseDto ToDerivedDto<TAggregate>(this TAggregate a)
            where TAggregate : class, IAggregate
        {
            return a switch
            {
                Customer c => c.ToDto(),
                Message m => m.ToDto(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
