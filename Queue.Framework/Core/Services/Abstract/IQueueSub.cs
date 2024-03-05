using MassTransit;

namespace Queue.Framework.Core.Services.Abstract
{
    public interface IQueueSub<TMessage> : IConsumer<TMessage>
        where TMessage : class, new()
    {
    }
}
