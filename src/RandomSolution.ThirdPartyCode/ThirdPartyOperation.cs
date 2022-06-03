using Microsoft.Extensions.Logging;

namespace RandomSolution.ThirdPartyCode;

public class ThirdPartyOperation
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
