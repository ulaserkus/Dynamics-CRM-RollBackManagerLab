namespace Queue.Framework.Core.Services.Abstract
{
    public interface IQueuePub<T>
        where T : class
    {

        Task AddDataToQueueAsync(T data);
    }
}
