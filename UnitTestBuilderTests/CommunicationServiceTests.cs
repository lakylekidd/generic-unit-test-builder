using FluentAssertions;
using Moq;
using UnitTestBuilder;
using UnitTestBuilder.Concrete;
using UnitTestBuilder.Mappers;
using UnitTestBuilder.Models;
using UnitTestBuilderTests.Builders;

namespace UnitTestBuilderTests
{
    public class CommunicationServiceTests : BaseServiceTests<CommunicationService, MessageDto, Message, CommunicationServiceBuilder, ICommunicationRepository>
    {
        [Fact]
        public async Task SendAsync_should_throw_when_no_message_provided()
        {
            var service = Builder.Build().Create();

            var action = async () => await service.SendAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SendAsync_should_fail_to_send()
        {
            var service = Builder
                .WithAggregate(out var message)
                .WithId()
                .Build().Create();

            var result = await service.SendAsync(message.ToDto());

            result.Should().BeFalse();
            Builder.MessagingServiceMock.Verify(x => x.SendAsync(It.IsAny<string>()), Times.Once);
            Builder.RepositoryMock.Verify(x => x.CreateAsync(message), Times.Never);
        }

        [Fact]
        public async Task SendAsync_should_send_successfully_and_persist()
        {
            var service = Builder
                .WithSuccess()
                .WithAggregate(out var message)
                .WithId()
                .Build().Create();

            var result = await service.SendAsync(message.ToDto());

            result.Should().BeTrue();
            Builder.MessagingServiceMock.Verify(x => x.SendAsync(It.IsAny<string>()), Times.Once);
            Builder.RepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Message>()), Times.Once);
        }

        protected override void CompareResultingCustomerAndVerifyMappings(Message expected, MessageDto resulted)
        {
            resulted.Should().NotBeNull();
            resulted!.To.Should().Be(expected.To);
            resulted!.From.Should().Be(expected.From);
            resulted!.Body.Should().Be(expected.Body);
        }
    }
}
