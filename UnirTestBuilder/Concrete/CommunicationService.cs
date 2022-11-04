using System.Text.Json;
using UnitTestBuilder.Core;
using UnitTestBuilder.Mappers;
using UnitTestBuilder.Models;

namespace UnitTestBuilder.Concrete
{
    public class CommunicationService : ServiceBase<MessageDto, Message>, ICommunicationService
    {
        private readonly IMessagingService _messagingService;

        public CommunicationService(
            IMessagingService messagingService,
            IIdentityGenerator identityGenerator,
            ICommunicationRepository repository) 
            : base(identityGenerator, repository)
        {
            _messagingService = messagingService;
        }

        public async Task<bool> SendAsync(IMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);
            var msgJson = JsonSerializer.Serialize(message);
            var result = await _messagingService.SendAsync(msgJson);
            var id = IdentityGenerator.Generate();

            if (result) await Repository.CreateAsync(message.ToAggregate(id));
            return result;
        }
    }
}
