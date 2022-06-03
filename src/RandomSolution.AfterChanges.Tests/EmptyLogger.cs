using Microsoft.Extensions.Logging;

namespace RandomSolution.AfterChanges.Tests;

internal class EmptyLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new Disposable();
    }
    
    private class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
