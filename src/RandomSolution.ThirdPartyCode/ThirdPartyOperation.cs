using Microsoft.Extensions.Logging;

namespace RandomSolution.ThirdPartyCode;

public class ThirdPartyOperation : IThirdPartyOperation
{
    private readonly ILogger _logger;

    public ThirdPartyOperation(ILogger logger)
    {
        _logger = logger;
    }

    public T DoWithOptionalBackupConnection<T, TClient>(
        TClient mainClient, 
        TClient backupClient, 
        Func<TClient, T> action)
    {
        Thread.Sleep(30_000);
        // here should be some code that we don't know
        // it is heavy and computationally demanding
        
        try
        {
            return action(mainClient);
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, "Error when invoking action on main client");
        }

        return action(backupClient);
    }
}
