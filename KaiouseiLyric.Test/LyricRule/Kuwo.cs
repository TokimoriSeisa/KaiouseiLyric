using KaiouseiLyric.Shared.LyricRule;

namespace KaiouseiLyric.Test.LyricRule;

public class Kuwo
{
    private readonly KuwoMusicLyricRule _rule = new();

    [SetUp]
    public void Setup()
    {
        _rule.DetectWindow();
    }

    [Test]
    public void FindWindow()
    {
        var window = _rule.DetectWindow();
        Assert.That(window, Is.Not.Null);
    }

    [Test]
    public void SetIsCaptureable()
    {
        _rule.IsCaptureable = true;
        Assert.That(_rule.IsCaptureable, Is.True);
        _rule.IsCaptureable = false;
        Assert.That(_rule.IsCaptureable, Is.False);
    }

    [Test]
    public void ObsName()
    {
        Assert.That(_rule.ObsWindowName, Is.EqualTo("[kwmusic.exe]: (null)"));
    }
}