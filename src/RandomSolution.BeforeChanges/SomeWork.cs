using Microsoft.Extensions.Logging;
using RandomSolution.ThirdPartyCode;

namespace RandomSolution.BeforeChanges;

public class SomeWork
{
    private readonly ILogger _logger;

    public SomeWork(ILogger logger)
    {
        _logger = logger;
    }

    public string Do(IClient mainClient, IClient backupClient)
    {
        var operation = new ThirdPartyOperation(_logger);
        
        var first = operation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("First"));
        
        var second = operation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("Second"));
        
        var third = operation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("Third"));
        
        return first + second + third;
    }
}
