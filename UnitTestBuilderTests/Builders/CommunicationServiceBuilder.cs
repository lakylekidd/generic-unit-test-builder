using Moq;
using UnitTestBuilder;
using UnitTestBuilder.Concrete;
using UnitTestBuilder.Models;

namespace UnitTestBuilderTests.Builders
{
    public class CommunicationServiceBuilder : BaseBuilder<CommunicationService, MessageDto, Message, ICommunicationRepository>
    {
        public Mock<IMessagingService> MessagingServiceMock { get; } = new();
        public IMessagingService MessagingService => MessagingServiceMock.Object;

        public override BaseBuilder<CommunicationService, MessageDto, Message, ICommunicationRepository> Build()
        {
            Services = new object[] { MessagingService, IdentityGenerator, Repository };
            return this;
        }

        public CommunicationServiceBuilder WithSuccess()
        {
            MessagingServiceMock.Setup(x => x.SendAsync(It.IsAny<string>())).ReturnsAsync(true);
            return this;
        }

        protected override Message GenerateAggregate(int withId)
        {
            return new Message()
            {
                Id = withId,
                Body = Guid.NewGuid().ToString()[..10],
                From = $"{Guid.NewGuid().ToString()[..3]}@email.com",
                To = $"{Guid.NewGuid().ToString()[..3]}@email.com"
            };
        }
    }
}
