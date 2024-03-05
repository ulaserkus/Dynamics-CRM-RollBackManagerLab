using MassTransit;
using Queue.Framework.Core.Services.Abstract;

namespace Queue.Framework.Core.Services.Concrete
{
    public class QueueSub<TMessage> : IQueueSub<TMessage>
        where TMessage : class, new()
    {
        public virtual Task Consume(ConsumeContext<TMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
