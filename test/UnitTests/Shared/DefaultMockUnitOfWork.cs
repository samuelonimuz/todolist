using Domaincrafters.Application;

namespace UnitTests.Shared;

public class DefaultMockUnitOfWork : IUnitOfWork
{
    public int DoCallCount { get; private set; }
    public Exception? ThrowOnDo { get; set; }

    public Task Do(Func<Task>? action = null)
    {
        if (ThrowOnDo != null)
            throw ThrowOnDo;

        DoCallCount++;

        action?.Invoke();

        return Task.CompletedTask;
    }
}
