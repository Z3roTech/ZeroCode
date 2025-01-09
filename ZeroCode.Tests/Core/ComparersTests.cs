using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.Core;

public class ComparersTests
{
    [Test]
    public void GenericComparerForTestingClassTest()
    {
        var comparer = new GenericEqualityComparer<TestingClass>(
            (left, right) => left.Id == right.Id && left.Guid == right.Guid,
            @class => [@class.Guid, @class.Id]
        );

        var commonGuid = Guid.NewGuid();
        var commonId = Random.Shared.Next();

        Assert.That(comparer.Equals(
                new TestingClass { Guid = commonGuid, Id = commonId },
                new TestingClass { Guid = commonGuid, Id = commonId }),
            Is.True
        );

        var list = new List<TestingClass>
        {
            new() { Guid = commonGuid, Id = commonId },
            new() { Guid = commonGuid, Id = commonId },
            new() { Guid = commonGuid, Id = commonId },
            new() { Guid = commonGuid, Id = commonId },
            new() { Guid = commonGuid, Id = commonId },
            new() { Guid = commonGuid, Id = commonId },
            TestingClass.CreateRandomNew(),
            TestingClass.CreateRandomNew(),
            TestingClass.CreateRandomNew()
        };

        // without comparer
        var actualList = list.Distinct().ToArray();
        Assert.That(actualList, Has.Length.EqualTo(list.Count));

        // with comparer
        actualList = list.Distinct(comparer).ToArray();
        Assert.That(actualList, Has.Length.EqualTo(4));
    }
}