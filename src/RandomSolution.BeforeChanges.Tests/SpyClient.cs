using System.Runtime.CompilerServices;

namespace RandomSolution.BeforeChanges.Tests;

internal class SpyClient : IClient
{
    private readonly List<string> _getDataParameters = new();
    private readonly Func<string, string> _getDataFunc;

    public SpyClient(Func<string, string> getDataFunc)
    {
        _getDataFunc = getDataFunc;
    }

    public IReadOnlyList<string> GetDataMethodParameters => _getDataParameters;

    public int GetDataCalledTimes => _getDataParameters.Count;
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public string GetData(string parameter)
    {
        _getDataParameters.Add(parameter);
        
        return _getDataFunc(parameter);
    }
}
