using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace RandomSolution.BeforeChanges.Tests;

[TestFixture]
public class DoWorkTests
{
    private readonly ILogger _logger = new EmptyLogger();

    [Test]
    public void main_client_works_correctly_and_backup_client_should_not_be_used()
    {
        var someWork = new SomeWork(_logger);

        var mainClient = new SpyClient(_ => "result from main");
        var backupClient = new SpyClient(_ => "result from backup");
        
        _ = someWork.Do(mainClient, backupClient);
        
        Assert.That(mainClient.GetDataCalledTimes, Is.EqualTo(3));
        Assert.That(backupClient.GetDataCalledTimes, Is.Zero);
    }

    [Test]
    public void main_client_throws_and_backup_client_should_be_used()
    {
        var someWork = new SomeWork(_logger);

        var mainClient = new SpyClient(_ => throw new InvalidOperationException());
        var backupClient = new SpyClient(_ => "result from backup");
        
        _ = someWork.Do(mainClient, backupClient);
        
        Assert.That(mainClient.GetDataCalledTimes, Is.EqualTo(3));
        Assert.That(backupClient.GetDataCalledTimes, Is.EqualTo(3));
    }
}
