using UnitTestBuilder.Core;
using UnitTestBuilder.Models;

namespace UnitTestBuilder
{
    public interface ICommunicationService : IService<MessageDto>
    {
        Task<bool> SendAsync(IMessage message);
    }
}
