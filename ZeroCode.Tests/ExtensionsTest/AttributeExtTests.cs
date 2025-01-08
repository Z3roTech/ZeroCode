using System.Reflection;
using ZeroCode.Extensions;
using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.ExtensionsTest;

[TestingSingle("This is a class!")]
[TestingMultiple(0)]
[TestingMultiple(1)]
[TestingMultiple(2)]
public class AttributeExtTests
{
    [TestingSingle("This is a field!")]
    public readonly int FieldWithAttribute = 0;

    [TestingSingle("This is a property!")]
    public int PropertyWithAttribute { get; set; } = 0;

    [TestingSingle("This is method!")]
    [TestingMultiple(0)]
    [TestingMultiple(1)]
    [TestingMultiple(2)]
    public void MethodWithAttribute() { }

    [Test]
    public void GetAttributeOfClassTest()
    {
        var singleAttribute = typeof(AttributeExtTests).GetAttribute<TestingSingleAttribute>();
        Assert.That(singleAttribute, Is.Not.Null);
        Assert.That(singleAttribute, Has.Property("Value").EqualTo("This is a class!"));

        // Get single attribute when multiple defines is forbidden
        Assert.Throws<AmbiguousMatchException>(() =>
            typeof(AttributeExtTests).GetAttribute<TestingMultipleAttribute>());

        var multipleAttributes = typeof(AttributeExtTests).GetAttributes<TestingMultipleAttribute>();
        Assert.That(multipleAttributes, Is.Not.Null.And.Not.Empty);
        var expected = new[] { 0, 1, 2 };
        for (var i = 0; i < multipleAttributes.Length; i++)
            Assert.That(multipleAttributes[i]!.Id, Is.EqualTo(expected[i]));
    }

    [Test]
    public void GetAttributeOfMemberClassTest()
    {
        // only works with public members
        var singleAttribute = typeof(AttributeExtTests)
            .GetAttribute<TestingSingleAttribute>(nameof(FieldWithAttribute));

        Assert.That(singleAttribute, Is.Not.Null);
        Assert.That(singleAttribute, Has.Property("Value").EqualTo("This is a field!"));

        singleAttribute = typeof(AttributeExtTests)
            .GetAttribute<TestingSingleAttribute>(nameof(PropertyWithAttribute));

        Assert.That(singleAttribute, Is.Not.Null);
        Assert.That(singleAttribute, Has.Property("Value").EqualTo("This is a property!"));

        singleAttribute = typeof(AttributeExtTests)
            .GetAttribute<TestingSingleAttribute>(nameof(MethodWithAttribute));

        Assert.That(singleAttribute, Is.Not.Null);
        Assert.That(singleAttribute, Has.Property("Value").EqualTo("This is method!"));

        var multipleAttributes = typeof(AttributeExtTests)
            .GetAttributes<TestingMultipleAttribute>(nameof(MethodWithAttribute));

        Assert.That(multipleAttributes, Is.Not.Null.And.Not.Empty);
        var expected = new[] { 0, 1, 2 };
        for (var i = 0; i < multipleAttributes.Length; i++)
            Assert.That(multipleAttributes[i]!.Id, Is.EqualTo(expected[i]));
    }

    [Test]
    public void GetDescriptionOfEnumValueTest()
    {
        var enumValueDescription = TestingEnum.Foo.GetDescription();
        Assert.That(enumValueDescription, Is.EqualTo("Description of Foo"));
    }
}