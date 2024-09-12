namespace Application.ExternalServices.BackgroundTák
{
    public interface IBackgroundTaskQueue
    {
        Task QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
