using ZeroCode.Extensions;
using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.ExtensionsTest;

public class EnumerableExtensionsTests
{
    [Test]
    public void SafeConcatenationTest()
    {
        int[]? firstSequence = null;
        int[]? secondSequence = null;

        var actualSequence = firstSequence.SafeConcat(secondSequence).ToArray();
        Assert.That(actualSequence, Is.Not.Null.And.Empty);

        secondSequence = [1, 2, 3];
        int[] expectedSequence = [1, 2, 3];
        actualSequence = firstSequence.SafeConcat(secondSequence).ToArray();
        Assert.That(actualSequence, Is.Not.Null.And.Not.Empty);
        for (var i = 0; i < expectedSequence.Length; i++)
            Assert.That(actualSequence[i], Is.EqualTo(expectedSequence[i]));

        firstSequence = [1, 2, 3];
        secondSequence = [3, 4, 5];
        expectedSequence = [1, 2, 3, 3, 4, 5];
        actualSequence = firstSequence.SafeConcat(secondSequence).ToArray();
        Assert.That(actualSequence, Is.Not.Null.And.Not.Empty);
        for (var i = 0; i < expectedSequence.Length; i++)
            Assert.That(actualSequence[i], Is.EqualTo(expectedSequence[i]));
    }

    [Test]
    public void ComputeHashCodeTest()
    {
        var firstSequence = new List<string> { "first", "second", "third" };
        var secondSequence = new List<string> { "foo", "bar", "foobar" };

        Assert.That(firstSequence.GetCollectionHashCode(), Is.Not.EqualTo(secondSequence.GetCollectionHashCode()));

        secondSequence = ["first", "second", "third"];
        Assert.That(firstSequence.GetCollectionHashCode(), Is.EqualTo(secondSequence.GetCollectionHashCode()));
    }

    [Test]
    public void MergeCollectionsTest()
    {
        var firstSeq = new List<TestingClass>
        {
            new() { Id = 1, Guid = new Guid("68C2ED91-88FB-467E-92C3-060D7390133D") },
            new() { Id = 2, Guid = new Guid("6F5EB6A7-8C1E-4F02-B87F-E1F312D80A4F") },
            new() { Id = 3, Guid = new Guid("61E4AA37-CA67-40C6-A11E-0097A4A21ADF") }
        };

        var secondSeq = new List<TestingClass>
        {
            new() { Id = 3, Guid = new Guid("87BE805D-0E5A-48C2-9A01-F4DCA1AB597A") },
            new() { Id = 4, Guid = Guid.Empty },
            new() { Id = 5, Guid = Guid.Empty }
        };

        var expectedSeq = new List<TestingClass>
        {
            new() { Id = 1, Guid = new Guid("68C2ED91-88FB-467E-92C3-060D7390133D") },
            new() { Id = 2, Guid = new Guid("6F5EB6A7-8C1E-4F02-B87F-E1F312D80A4F") },
            new() { Id = 3, Guid = new Guid("87BE805D-0E5A-48C2-9A01-F4DCA1AB597A") },
            new() { Id = 4, Guid = Guid.Empty },
            new() { Id = 5, Guid = Guid.Empty }
        };

        var actualSeq = firstSeq.Merge(secondSeq, @class => @class.Id, MergingFunc)
            .ToArray();

        Assert.That(actualSeq, Is.Not.Null.And.Not.Empty);
        Assert.That(actualSeq, Has.Length.EqualTo(expectedSeq.Count));
        for (var i = 0; i < actualSeq.Length; i++)
        {
            var actual = actualSeq[i];
            var expected = expectedSeq[i];
            Assert.That(actual, Has.Property(nameof(actual.Id)).EqualTo(expected.Id));
            Assert.That(actual, Has.Property(nameof(actual.Guid)).EqualTo(expected.Guid));
        }

        return;

        TestingClass MergingFunc(TestingClass left, TestingClass right)
        {
            return new TestingClass { Guid = right.Guid, Id = right.Id };
        }
    }
}