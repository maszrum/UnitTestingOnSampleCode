using RandomSolution.ThirdPartyCode;

namespace RandomSolution.BeforeChanges;

public class SomeWork
{
    private readonly IThirdPartyOperation _thirdPartyOperation;

    public SomeWork(IThirdPartyOperation thirdPartyOperation)
    {
        _thirdPartyOperation = thirdPartyOperation;
    }

    public string Do(IClient mainClient, IClient backupClient)
    {
        var first = _thirdPartyOperation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("First"));
        
        var second = _thirdPartyOperation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("Second"));
        
        var third = _thirdPartyOperation.DoWithOptionalBackupConnection(
            mainClient, 
            backupClient, 
            client => client.GetData("Third"));
        
        return first + second + third;
    }
}
