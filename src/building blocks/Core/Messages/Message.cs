namespace Core.Messages
{
    public abstract class Message
    {
        public string MessageType => GetType().Name;
        public Guid AggregatedId { get; protected set; }
    }
}
