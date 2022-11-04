namespace UnitTestBuilder.Core
{
    public interface IMessage
    {
        string To { get; }
        string From { get; }
        string Body { get; }
    }
}
