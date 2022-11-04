using UnitTestBuilder.Core;
using UnitTestBuilder.Models;

namespace UnitTestBuilder.Mappers
{
    public static class MessageMapper
    {
        public static Message ToAggregate(this IMessage m, int id) => new()
        {
            Id = id,
            Body = m.Body,
            From = m.From,
            To = m.To
        };

        public static MessageDto ToDto(this Message m) => new()
        {
            Id = m.Id,
            To = m.To,
            From = m.From,
            Body = m.Body
        };
    }
}
