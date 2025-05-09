using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using KaiouseiLyric.Shared.Utils;
using Vanara.PInvoke;

namespace KaiouseiLyric.Shared.LyricRule;

public abstract partial class LyricRule : ObservableObject
{
    /// <summary>
    ///     获取当前歌词规则所属的应用程序名称。
    /// </summary>
    /// <remarks>
    ///     此属性返回实现具体歌词规则的应用程序名称，
    ///     例如 "QQ音乐" 表示此实例对应的逻辑规则适用于 QQ 音乐应用。
    /// </remarks>
    [ObservableProperty] private string _appName = string.Empty;

    /// <summary>
    ///     获取或设置是否启用自动检测功能。
    /// </summary>
    [ObservableProperty] private bool _autoCapture = true;

    /// <summary>
    ///     获取或设置是否启用自动检测功能。
    /// </summary>
    [ObservableProperty] private bool _autoDetect = true;

    /// <summary>
    ///     获取当前歌词规则的唯一键值。
    /// </summary>
    [ObservableProperty] private string _key = string.Empty;

    /// <summary>
    ///     指示当前窗口是否可被截取（捕获）。
    /// </summary>
    public bool IsCaptureable
    {
        get
        {
            if (FoundWindow == null) return false;

            var exStyle = User32.GetWindowLong(FoundWindow.Handle, User32.WindowLongFlags.GWL_EXSTYLE);
            return (exStyle & (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW) !=
                   (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;
        }
        set
        {
            if (FoundWindow == null) return;

            if (value)
            {
                var exStyle = User32.GetWindowLong(FoundWindow.Handle, User32.WindowLongFlags.GWL_EXSTYLE);
                exStyle &= ~(int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;
                exStyle |= (int)User32.WindowStylesEx.WS_EX_APPWINDOW;
                User32.SetWindowLong(FoundWindow.Handle, User32.WindowLongFlags.GWL_EXSTYLE, exStyle);
            }
            else
            {
                var exStyle = User32.GetWindowLong(FoundWindow.Handle, User32.WindowLongFlags.GWL_EXSTYLE);
                exStyle |= (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;
                User32.SetWindowLong(FoundWindow.Handle, User32.WindowLongFlags.GWL_EXSTYLE, exStyle);
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     获取当前捕获的窗口信息。
    /// </summary>
    public FoundWindow? FoundWindow { get; set; }

    /// <summary>
    ///     表示当前窗口是否已失效。
    /// </summary>
    public bool IsWindowDead
    {
        get
        {
            if (FoundWindow == null) return true;

            if (FoundWindow.Handle == IntPtr.Zero) return true;

            return !User32.IsWindow(FoundWindow.Handle);
        }
    }


    /// <summary>
    ///     获取当前检测到的窗口的描述信息。
    ///     如果找到窗口信息，将返回 "[进程名]: 窗口标题" 格式的字符串；
    ///     如果未找到窗口信息，则返回空字符串。
    /// </summary>
    /// <returns>描述窗口的字符串，格式为 "[进程名]: 窗口标题"；如果未找到窗口信息，返回空字符串。</returns>
    public string ObsWindowName
    {
        get
        {
            if (FoundWindow == null) return string.Empty;

            _ = User32.GetWindowThreadProcessId(FoundWindow.Handle, out var processId);
            using var process = Process.GetProcessById((int)processId);
            var windowName = string.IsNullOrEmpty(FoundWindow.WindowName) ? "(null)" : FoundWindow.WindowName;
            return $"[{process.ProcessName}.exe]: {windowName}";
        }
    }

    /// <summary>
    ///     检测并返回符合条件的窗口信息。
    ///     将通过定义的规则筛选满足条件的窗口。具体规则需要实现类进行定义和处理。
    ///     如果检测到符合条件的窗口，返回符合条件的 <see cref="FoundWindow" /> 对象，并可能更新实例的相关属性；
    ///     如果未找到符合条件的窗口，返回 null。
    /// </summary>
    /// <returns>符合条件的 <see cref="FoundWindow" /> 对象；如果未找到，返回 null。</returns>
    public abstract FoundWindow? DetectWindow();

    public void RefreshStatus()
    {
        OnPropertyChanged(nameof(ObsWindowName));
        OnPropertyChanged(nameof(IsWindowDead));
        OnPropertyChanged(nameof(IsCaptureable));
    }
}