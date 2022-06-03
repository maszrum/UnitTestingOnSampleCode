using System.Runtime.CompilerServices;

namespace RandomSolution.AfterChanges.Tests;

internal class SpyClient : IClient
{
    private readonly List<string> _getDataParameters = new();
    private readonly Func<string, Task<string>> _getDataFunc;

    public SpyClient(Func<string, Task<string>> getDataFunc)
    {
        _getDataFunc = getDataFunc;
    }

    public IReadOnlyList<string> GetDataMethodParameters => _getDataParameters;

    public int GetDataCalledTimes => _getDataParameters.Count;
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Task<string> GetData(string parameter)
    {
        _getDataParameters.Add(parameter);
        
        return _getDataFunc(parameter);
    }
}
