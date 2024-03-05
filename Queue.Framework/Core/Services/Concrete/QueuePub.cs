using MassTransit;
using Queue.Framework.Core.Services.Abstract;

namespace Queue.Framework.Core.Services.Concrete
{
    public class QueuePub<T> : IQueuePub<T>
        where T : class
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public QueuePub(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task AddDataToQueueAsync(T data)
        {
            await _publishEndpoint.Publish(data);
        }
    }
}
