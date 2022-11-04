using UnitTestBuilder.Core;

namespace UnitTestBuilder.Models
{
    public class MessageDto : BaseDto, IMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
    }
}
