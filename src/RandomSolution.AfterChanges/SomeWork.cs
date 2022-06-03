using Microsoft.Extensions.Logging;
using RandomSolution.ThirdPartyCode;

namespace RandomSolution.AfterChanges;

public class SomeWork
{
    private readonly ILogger _logger;

    public SomeWork(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<string> Do(IClient mainClient, IClient backupClient)
    {
        var operation = new ThirdPartyOperation(_logger);
        
        var first = await operation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("First"));
        
        var second = await operation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("Second"));
        
        var third = await operation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("Third"));
        
        return first + second + third;
    }
}
