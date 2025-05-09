using System.Diagnostics;
using KaiouseiLyric.Shared.Utils;
using Vanara.PInvoke;

namespace KaiouseiLyric.Shared.LyricRule;

public class KuwoMusicLyricRule : LyricRule
{
    public KuwoMusicLyricRule()
    {
        Key = "KuwoMusicLyricRule";
        AppName = "酷我音乐";
        AutoDetect = true;
        AutoCapture = true;
    }

    public override FoundWindow? DetectWindow()
    {
        var windows = Window.FindAllWindowsByClassName("KwDeskLyricTopWnd", IntPtr.Zero);

        foreach (var window in windows)
        {
            _ = User32.GetWindowThreadProcessId(window.Handle, out var processId);

            try
            {
                using var process = Process.GetProcessById((int)processId);
                if (!process.ProcessName.Equals("kwmusic", StringComparison.OrdinalIgnoreCase)) continue;
                FoundWindow = window;
                return window;
            }
            catch (ArgumentException)
            {
            }
        }

        FoundWindow = null;
        return null;
    }
}