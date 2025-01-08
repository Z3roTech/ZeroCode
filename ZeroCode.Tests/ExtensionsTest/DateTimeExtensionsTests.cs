using ZeroCode.Extensions;

namespace ZeroCode.Tests.ExtensionsTest;

public class DateTimeExtensionsTests
{
    [Test]
    public void ExtensionsMethodForDateTimeTest()
    {
        var dt = DateTime.Parse("2024-10-23T15:35:20.123");
        // End of day / Start of day 
        Assert.That(dt.EndOfDay(), Is.EqualTo(new DateTime(
            2024,
            10,
            23,
            DateTime.MaxValue.Hour,
            DateTime.MaxValue.Minute,
            DateTime.MaxValue.Second,
            DateTime.MaxValue.Millisecond
        )));

        Assert.That(dt.StartOfDay(), Is.EqualTo(new DateTime(2024, 10, 23)));

        // DateTime truncating
        Assert.That(dt.Truncate(DateTimePart.Millisecond), Is.EqualTo(DateTime.Parse("2024-10-23T15:35:20.123")));
        Assert.That(dt.Truncate(DateTimePart.Second), Is.EqualTo(DateTime.Parse("2024-10-23T15:35:20")));
        Assert.That(dt.Truncate(DateTimePart.Minute), Is.EqualTo(DateTime.Parse("2024-10-23T15:35:00")));
        Assert.That(dt.Truncate(DateTimePart.Hour), Is.EqualTo(DateTime.Parse("2024-10-23T15:00:00")));
        Assert.That(dt.Truncate(DateTimePart.Day), Is.EqualTo(DateTime.Parse("2024-10-23T00:00:00")));
        Assert.That(dt.Truncate(DateTimePart.Month), Is.EqualTo(DateTime.Parse("2024-10-01T00:00:00")));
        Assert.That(dt.Truncate(DateTimePart.Year), Is.EqualTo(DateTime.Parse("2024-01-01T00:00:00")));

        // Min / Max
        var secondDt = DateTime.Parse("2024-11-10T21:40:15.990");
        Assert.That(DateTimeExtensions.Max(dt, secondDt), Is.EqualTo(secondDt));
        Assert.That(DateTimeExtensions.Min(dt, secondDt), Is.EqualTo(dt));

        // Clamp
        Assert.That(DateTimeExtensions.Clamp(DateTime.Now, dt, secondDt),
            Is.EqualTo(secondDt));

        Assert.That(DateTimeExtensions.Clamp(DateTime.Now.AddYears(-1000), dt, secondDt),
            Is.EqualTo(dt));

        Assert.That(DateTimeExtensions.Clamp(DateTime.Parse("2024-11-01T12:30:05.300"), dt, secondDt),
            Is.EqualTo(DateTime.Parse("2024-11-01T12:30:05.300")));
    }

    [Test]
    public void ExtensionsMethodForDateTimeOffsetTest()
    {
        var dt = DateTimeOffset.Parse("2024-10-23T15:35:20.123+08:00");

        // End of day / Start of day 
        Assert.That(dt.EndOfDay(), Is.EqualTo(new DateTimeOffset(
            2024,
            10,
            23,
            DateTime.MaxValue.Hour,
            DateTime.MaxValue.Minute,
            DateTime.MaxValue.Second,
            DateTime.MaxValue.Millisecond,
            TimeSpan.FromHours(8)
        )));

        Assert.That(dt.StartOfDay(), Is.EqualTo(new DateTimeOffset(
            new DateTime(2024, 10, 23),
            TimeSpan.FromHours(8)
        )));

        // DateTime truncating
        Assert.That(dt.Truncate(DateTimePart.Millisecond),
            Is.EqualTo(DateTimeOffset.Parse("2024-10-23T15:35:20.123+08:00")));

        Assert.That(dt.Truncate(DateTimePart.Second),
            Is.EqualTo(DateTimeOffset.Parse("2024-10-23T15:35:20+08:00")));

        Assert.That(dt.Truncate(DateTimePart.Minute),
            Is.EqualTo(DateTimeOffset.Parse("2024-10-23T15:35:00+08:00")));

        Assert.That(dt.Truncate(DateTimePart.Hour),
            Is.EqualTo(DateTimeOffset.Parse("2024-10-23T15:00:00+08:00")));

        Assert.That(dt.Truncate(DateTimePart.Day),
            Is.EqualTo(DateTimeOffset.Parse("2024-10-23T00:00:00+08:00")));

        Assert.That(dt.Truncate(DateTimePart.Month),
            Is.EqualTo(DateTimeOffset.Parse("2024-10-01T00:00:00+08:00")));

        Assert.That(dt.Truncate(DateTimePart.Year),
            Is.EqualTo(DateTimeOffset.Parse("2024-01-01T00:00:00+08:00")));

        // Min / Max
        var secondDt = DateTimeOffset.Parse("2024-11-10T21:40:15.990-05:00");
        Assert.That(DateTimeExtensions.Max(dt, secondDt), Is.EqualTo(secondDt));
        Assert.That(DateTimeExtensions.Min(dt, secondDt), Is.EqualTo(dt));

        // Clamp
        Assert.That(DateTimeExtensions.Clamp(DateTimeOffset.Now, dt, secondDt),
            Is.EqualTo(secondDt));

        Assert.That(DateTimeExtensions.Clamp(DateTimeOffset.Now.AddYears(-1000), dt, secondDt),
            Is.EqualTo(dt));

        Assert.That(DateTimeExtensions.Clamp(DateTimeOffset.Parse("2024-11-01T12:30:05.300+01:00"), dt, secondDt),
            Is.EqualTo(DateTimeOffset.Parse("2024-11-01T12:30:05.300+01:00")));
    }
}