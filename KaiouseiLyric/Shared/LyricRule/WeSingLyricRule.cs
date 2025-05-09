using System.Diagnostics;
using KaiouseiLyric.Shared.Utils;
using Vanara.PInvoke;

namespace KaiouseiLyric.Shared.LyricRule;

public class WeSingLyricRule : LyricRule
{
    public WeSingLyricRule()
    {
        Key = "WeSingMusicLyricRule";
        AppName = "全民K歌（歌词）";
        AutoCapture = true;
        AutoCapture = true;
    }

    public override FoundWindow? DetectWindow()
    {
        var windows = Window.FindAllWindows("CLyricRenderWnd", IntPtr.Zero)
            .Where(x => x.ClassName.Contains("ATL:"))
            .ToList();

        foreach (var window in windows)
        {
            _ = User32.GetWindowThreadProcessId(window.Handle, out var processId);

            try
            {
                using var process = Process.GetProcessById((int)processId);
                if (!process.ProcessName.Equals("WeSing", StringComparison.OrdinalIgnoreCase)) continue;
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