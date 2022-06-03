using Moq;
using NUnit.Framework;
using RandomSolution.ThirdPartyCode;

namespace RandomSolution.AfterChanges.Tests;

[TestFixture]
public class DoWorkTests
{
    private readonly Mock<IThirdPartyOperation> _mockedThirdPartyOperation;

    public DoWorkTests()
    {
        _mockedThirdPartyOperation = new Mock<IThirdPartyOperation>();
        
        _mockedThirdPartyOperation
            .Setup(m => m.DoWithOptionalBackupConnection(
                It.IsAny<IClient>(), 
                It.IsAny<IClient>(), 
                It.IsAny<Func<IClient, Task<string>>>()))
            .Returns<IClient, IClient, Func<IClient, Task<string>>>((mainClient, backupClient, action) =>
            {
                // simplified implementation of IThirdPartyOperation that mimics real one
                try
                {
                    return action(mainClient);
                }
                catch
                {
                    // we do not need logging in mock
                }

                return action(backupClient);
            });
    }

    [Test]
    public void main_client_works_correctly_and_backup_client_should_not_be_used()
    {
        var mainClient = new SpyClient(_ => Task.FromResult("result from main"));
        var backupClient = new SpyClient(_ => Task.FromResult("result from backup"));
        
        var someWork = new SomeWork(_mockedThirdPartyOperation.Object);
        
        _ = someWork.Do(mainClient, backupClient);
        
        Assert.That(mainClient.GetDataCalledTimes, Is.EqualTo(3));
        Assert.That(backupClient.GetDataCalledTimes, Is.Zero);
    }

    [Test]
    public void main_client_throws_and_backup_client_should_be_used()
    {
        var mainClient = new SpyClient(_ => Task.FromException<string>(new InvalidOperationException()));
        var backupClient = new SpyClient(_ =>  Task.FromResult("result from backup"));
        
        var someWork = new SomeWork(_mockedThirdPartyOperation.Object);
        
        _ = someWork.Do(mainClient, backupClient);
        
        Assert.That(mainClient.GetDataCalledTimes, Is.EqualTo(3));
        Assert.That(backupClient.GetDataCalledTimes, Is.EqualTo(3));
    }
}
