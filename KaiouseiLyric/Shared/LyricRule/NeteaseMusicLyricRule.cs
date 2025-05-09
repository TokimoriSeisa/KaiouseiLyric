using System.Diagnostics;
using KaiouseiLyric.Shared.Utils;
using Vanara.PInvoke;

namespace KaiouseiLyric.Shared.LyricRule;

public class NeteaseMusicLyricRule : LyricRule
{
    public NeteaseMusicLyricRule()
    {
        Key = "NeteaseMusicLyricRule";
        AppName = "网易云音乐";
    }

    public override FoundWindow? DetectWindow()
    {
        var windows = Window.FindAllWindowsByClassName("DesktopLyrics", IntPtr.Zero)
            .Where(x => x.WindowName.Contains("桌面歌词"))
            .ToList();

        foreach (var window in windows)
        {
            _ = User32.GetWindowThreadProcessId(window.Handle, out var processId);

            try
            {
                using var process = Process.GetProcessById((int)processId);
                if (!process.ProcessName.Equals("cloudmusic", StringComparison.OrdinalIgnoreCase)) continue;
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