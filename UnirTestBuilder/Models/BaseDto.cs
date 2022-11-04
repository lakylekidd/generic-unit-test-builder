using UnitTestBuilder.Core;

namespace UnitTestBuilder.Models
{
    public abstract class BaseDto : IAggregate
    {
        public int Id { get; set; }
    }
}
