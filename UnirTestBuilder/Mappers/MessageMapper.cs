using UnitTestBuilder.Models;

namespace UnitTestBuilder.Mappers
{
    public static class MessageMapper
    {
        public static MessageDto ToDto(this Message m) => new()
        {
            Id = m.Id
        };
    }
}
