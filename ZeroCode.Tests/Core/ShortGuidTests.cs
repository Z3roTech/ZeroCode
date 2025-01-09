namespace ZeroCode.Tests.Core;

public class ShortGuidTests
{
    [Test]
    public void TransformGuidToShortGuidAndViceVersaTest()
    {
        var guid = Guid.NewGuid();
        var shortGuid = new ShortGuid(guid);
        Assert.That(shortGuid, Is.Not.EqualTo(ShortGuid.Empty));
        Assert.That(shortGuid, Is.EqualTo(guid));

        var sourceGuid = shortGuid.ToGuid();
        Assert.That(sourceGuid, Is.EqualTo(guid));

        guid = new Guid("C101D2F0-ED1B-429A-8B4C-6F55454B1727");
        shortGuid = new ShortGuid("8NIBwRvtmkKLTG9VRUsXJw");

        Assert.That(shortGuid, Is.Not.EqualTo(ShortGuid.Empty));
        Assert.That(shortGuid, Is.EqualTo(guid));

        sourceGuid = shortGuid.ToGuid();
        Assert.That(sourceGuid, Is.EqualTo(guid));
    }
}