using System.Text.Json;
using UnitTestBuilder.Core;
using UnitTestBuilder.Models;

namespace UnitTestBuilder.Concrete
{
    public class CommunicationService : ServiceBase<MessageDto, Message>, ICommunicationService
    {
        private readonly IMessagingService _messagingService;

        public CommunicationService(
            IMessagingService messagingService,
            ICommunicationRepository repository) 
            : base(repository)
        {
            _messagingService = messagingService;
        }

        public async Task<bool> SendAsync(IMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);
            var msgJson = JsonSerializer.Serialize(message);
            return await _messagingService.SendAsync(msgJson);
        }
    }
}
