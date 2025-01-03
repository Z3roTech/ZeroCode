using System.Runtime.ExceptionServices;
using Microsoft.Identity.Client;
using ZeroCode.Extensions.Common;

namespace ZeroCode.Tests.ExtensionsTest;

public class ObjectExtensionsTests
{
    [Test]
    public void CastAndAsExtensionsTest()
    {
        var source = new List<int> { 0, 1, 2, 3, 4 };
        var actual = source.As<ICollection<int>>();
        Assert.That(actual, Is.Not.Null.And.Not.Empty.And.EqualTo(source));

        object number = 100;
        Assert.That(number.Cast<int>(), Is.EqualTo(number));

        object dbValue = DBNull.Value;
        Assert.That(dbValue.DbCast<string>(), Is.EqualTo(default(string)));
        dbValue = "hello";
        Assert.That(dbValue.DbCast<string>(), Is.EqualTo("hello"));
    }

    [Test]
    public void DefaultCheckAndRefSetterTest()
    {
        var notNullValue = 0L;
        Assert.That(notNullValue.IsDefault(), Is.True);
        ObjectExtensions.SetIfDefault(ref notNullValue, 100L);
        Assert.That(notNullValue, Is.EqualTo(100L));
        ObjectExtensions.SetIfDefault(ref notNullValue, 0L);
        Assert.That(notNullValue, Is.EqualTo(100L));

        var nullableValue = default(string);
        Assert.That(nullableValue.IsDefault(), Is.True);
        ObjectExtensions.SetIfDefault(ref nullableValue, "hello, world");
        Assert.That(nullableValue, Is.EqualTo("hello, world"));
        ObjectExtensions.SetIfDefault(ref nullableValue, null);
        Assert.That(nullableValue, Is.EqualTo("hello, world"));
    }
}