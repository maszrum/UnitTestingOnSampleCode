using Moq;
using NUnit.Framework;
using RandomSolution.ThirdPartyCode;

namespace RandomSolution.BeforeChanges.Tests;

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
                It.IsAny<Func<IClient, string>>()))
            .Returns<IClient, IClient, Func<IClient, string>>((mainClient, backupClient, action) =>
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
        var mainClient = new SpyClient(_ => "result from main");
        var backupClient = new SpyClient(_ => "result from backup");

        var someWork = new SomeWork(_mockedThirdPartyOperation.Object);

        _ = someWork.Do(mainClient, backupClient);

        Assert.That(mainClient.GetDataCalledTimes, Is.EqualTo(3));
        Assert.That(backupClient.GetDataCalledTimes, Is.Zero);
    }

    [Test]
    public void main_client_throws_and_backup_client_should_be_used()
    {
        var mainClient = new SpyClient(_ => throw new InvalidOperationException());
        var backupClient = new SpyClient(_ => "result from backup");

        var someWork = new SomeWork(_mockedThirdPartyOperation.Object);

        _ = someWork.Do(mainClient, backupClient);

        Assert.That(mainClient.GetDataCalledTimes, Is.EqualTo(3));
        Assert.That(backupClient.GetDataCalledTimes, Is.EqualTo(3));
    }
}
