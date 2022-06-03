namespace RandomSolution.ThirdPartyCode;

public interface IThirdPartyOperation
{
    T DoWithOptionalBackupConnection<T, TClient>(
        TClient mainClient, 
        TClient backupClient, 
        Func<TClient, T> action);
}
