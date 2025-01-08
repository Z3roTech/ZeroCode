using ZeroCode.Extensions;

namespace ZeroCode.Tests.ExtensionsTest;

public class DictionaryExtensionsTests
{
    [Test]
    public void AddOrUpdateExtensionTest()
    {
        var dictionary = new Dictionary<int, string> { [0] = "foobar" };

        dictionary.AddOrUpdate(0, "Lorem");
        dictionary.AddOrUpdate(1, "ipsum");
        Assert.That(dictionary, Has.Count.EqualTo(2));
        Assert.That(dictionary, Has.ItemAt(0).EqualTo("Lorem"));
        Assert.That(dictionary, Has.ItemAt(1).EqualTo("ipsum"));

        dictionary.AddOrUpdate(1, "dolor", "sit");
        dictionary.AddOrUpdate(2, "amet", "consectetur");
        Assert.That(dictionary, Has.Count.EqualTo(3));
        Assert.That(dictionary, Has.ItemAt(1).EqualTo("sit"));
        Assert.That(dictionary, Has.ItemAt(2).EqualTo("amet"));

        dictionary.AddOrUpdate(2, "consectetur", (_, _) => "adipiscing");
        dictionary.AddOrUpdate(3, "elit", (_, _) => "Vivamus");
        Assert.That(dictionary, Has.Count.EqualTo(4));
        Assert.That(dictionary, Has.ItemAt(2).EqualTo("adipiscing"));
        Assert.That(dictionary, Has.ItemAt(3).EqualTo("elit"));

        dictionary.AddOrUpdate(3, _ => "leo", (_, _) => "augue");
        dictionary.AddOrUpdate(4, _ => "viverra", (_, _) => "nec");
        Assert.That(dictionary, Has.Count.EqualTo(5));
        Assert.That(dictionary, Has.ItemAt(3).EqualTo("augue"));
        Assert.That(dictionary, Has.ItemAt(4).EqualTo("viverra"));

        dictionary.AddOrUpdate(4, "molestie", (_, _, arg) => arg, "ut");
        dictionary.AddOrUpdate(5, "laoreet", (_, _, arg) => arg, "nec");
        Assert.That(dictionary, Has.Count.EqualTo(6));
        Assert.That(dictionary, Has.ItemAt(4).EqualTo("ut"));
        Assert.That(dictionary, Has.ItemAt(5).EqualTo("laoreet"));

        dictionary.AddOrUpdate(5, _ => "velit", (_, _, arg) => arg, "Nulla");
        dictionary.AddOrUpdate(6, _ => "facilisi", (_, _, arg) => arg, "Phasellus");
        Assert.That(dictionary, Has.Count.EqualTo(7));
        Assert.That(dictionary, Has.ItemAt(5).EqualTo("Nulla"));
        Assert.That(dictionary, Has.ItemAt(6).EqualTo("facilisi"));
    }

    [Test]
    public void GetOrAddExtensionTest()
    {
        var dictionary = new Dictionary<int, string> { [0] = "Lorem" };

        Assert.That(dictionary.GetOrAdd(0, "ipsum"), Is.EqualTo("Lorem"));
        Assert.That(dictionary.GetOrAdd(1, "dolor"), Is.EqualTo("dolor"));
        Assert.That(dictionary, Has.Count.EqualTo(2));

        Assert.That(dictionary.GetOrAdd(1, _ => "sit"), Is.EqualTo("dolor"));
        Assert.That(dictionary.GetOrAdd(2, _ => "amet"), Is.EqualTo("amet"));
        Assert.That(dictionary, Has.Count.EqualTo(3));

        Assert.That(dictionary.GetOrAdd(2, (_, arg) => arg, "consectetur"), Is.EqualTo("amet"));
        Assert.That(dictionary.GetOrAdd(3, (_, arg) => arg, "adipiscing"), Is.EqualTo("adipiscing"));
        Assert.That(dictionary, Has.Count.EqualTo(4));
    }

    [Test]
    public void DictionaryTransformingTest()
    {
        var dictionary = new Dictionary<int, string>
        {
            [100] = "Hello, world",
            [200] = "FooBar",
            [300] = "Lorem ipsum..."
        };

        var expected = new Dictionary<int, object?>
        {
            [100] = "Hello, world",
            [200] = "FooBar",
            [300] = "Lorem ipsum..."
        };

        var abstractDict = dictionary.ToAbstractDictionary();
        Assert.That(abstractDict, Has.Count.EqualTo(expected.Count));
        foreach (var (key, value) in abstractDict) Assert.That(value, Is.EqualTo(expected[key]));
    }

    [Test]
    public void MultipleActionsOnToDictionary()
    {
        var dictionary = new Dictionary<int, string>
        {
            [0] = "foobar",
            [100] = "Welcome to hell!"
        };

        var otherDictionary = new Dictionary<int, string>
        {
            [100] = "Hello, world",
            [200] = "FooBar",
            [300] = "Lorem ipsum..."
        };

        var addEnumerable = new List<KeyValuePair<int, string>>
        {
            new(10, "Hi"),
            new(0, "ZeroCode"),
            new(-1, "Extensions")
        };

        var expectedDictionary = new Dictionary<int, string>
        {
            [0] = "foobar",
            [100] = "Welcome to hell!",
            [200] = "FooBar",
            [300] = "Lorem ipsum...",
            [10] = "Hi",
            [-1] = "Extensions"
        };

        dictionary.TryAddRange(otherDictionary);
        dictionary.TryAddRange(addEnumerable);
        Assert.That(dictionary, Has.Count.EqualTo(expectedDictionary.Count));
        foreach (var (key, value) in dictionary) Assert.That(value, Is.EqualTo(expectedDictionary[key]));

        dictionary = new Dictionary<int, string>
        {
            [0] = "Lorem ipsum",
            [1] = "dolor sit amet",
            [2] = "consectetur adipiscing elit"
        };

        otherDictionary = new Dictionary<int, string>
        {
            [2] = "Hello, world",
            [3] = "FooBar",
            [4] = "Lorem ipsum..."
        };

        addEnumerable =
        [
            new KeyValuePair<int, string>(4, "foo"),
            new KeyValuePair<int, string>(-1, "bar"),
            new KeyValuePair<int, string>(10, "ZeroCode")
        ];

        expectedDictionary = new Dictionary<int, string>
        {
            [0] = "Lorem ipsum",
            [1] = "dolor sit amet",
            [2] = "Hello, world",
            [3] = "FooBar",
            [4] = "foo",
            [10] = "ZeroCode",
            [-1] = "bar"
        };

        dictionary.TryAddOrUpdateRange(otherDictionary);
        dictionary.TryAddOrUpdateRange(addEnumerable);
        Assert.That(dictionary, Has.Count.EqualTo(expectedDictionary.Count));
        foreach (var (key, value) in dictionary) Assert.That(value, Is.EqualTo(expectedDictionary[key]));
    }
}