namespace RandomSolution.AfterChanges;

public interface IClient 
{
    Task<string> GetData(string parameter);
}
