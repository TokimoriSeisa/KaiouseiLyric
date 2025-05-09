using System.Collections.ObjectModel;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using KaiouseiLyric.Shared.LyricRule;
using Timer = System.Timers.Timer;

namespace KaiouseiLyric.Service;

public partial class WindowManager : ObservableObject
{
    /// <summary>
    ///     事件检查计时器循环时间
    /// </summary>
    private const int EventTimerInterval = 1000;

    /// <summary>
    ///     事件检查计时器
    /// </summary>
    private readonly Timer _eventTimer = new();

    /// <summary>
    ///     用于定义查找窗口规则的集合
    /// </summary>
    [ObservableProperty] private ObservableCollection<LyricRule> _lyricRules = [];

    private WindowManager()
    {
        LyricRules.Add(new QMusicLyricRule());
        LyricRules.Add(new NeteaseMusicLyricRule());
        LyricRules.Add(new KugouMusicLyricRule());
        LyricRules.Add(new KuwoMusicLyricRule());
        LyricRules.Add(new WeSingLyricRule());
        LyricRules.Add(new WeSingScoreRule());

        _eventTimer.Elapsed += EventTimer_Elapsed;
        _eventTimer.Interval = EventTimerInterval;
        _eventTimer.Enabled = true;
    }

    public static WindowManager Instance { get; } = new();

    /// <summary>
    ///     每隔 eventTimerInterval 搜索一次窗口，如果窗口未被监控，则加入。
    ///     如果窗口已经销毁则移除。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EventTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        foreach (var rule in LyricRules)
        {
            if (rule.AutoDetect)
                rule.DetectWindow();
            if (rule.AutoCapture) rule.IsCaptureable = true;
            rule.RefreshStatus();
        }
    }
}