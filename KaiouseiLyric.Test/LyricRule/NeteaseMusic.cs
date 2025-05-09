using KaiouseiLyric.Shared.LyricRule;

namespace KaiouseiLyric.Test.LyricRule;

public class NeteaseMusic
{
    private readonly NeteaseMusicLyricRule _rule = new();

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
        Assert.That(window.WindowName, Is.EqualTo("桌面歌词"));
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
        Assert.That(_rule.ObsWindowName, Is.EqualTo("[cloudmusic.exe]: 桌面歌词"));
    }
}