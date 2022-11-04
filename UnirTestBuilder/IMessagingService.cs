namespace UnitTestBuilder
{
    public interface IMessagingService
    {
        Task<bool> SendAsync(string message);
    }
}
