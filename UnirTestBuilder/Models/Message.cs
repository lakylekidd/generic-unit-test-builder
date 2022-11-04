using UnitTestBuilder.Core;

namespace UnitTestBuilder.Models
{
    public class Message : IAggregate
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public int Id { get; set; }
    }
}
