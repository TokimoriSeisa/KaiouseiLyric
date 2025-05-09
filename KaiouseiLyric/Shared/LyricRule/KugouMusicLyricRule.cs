using System.Diagnostics;
using KaiouseiLyric.Shared.Utils;
using Vanara.PInvoke;

namespace KaiouseiLyric.Shared.LyricRule;

public class KugouMusicLyricRule : LyricRule
{
    public KugouMusicLyricRule()
    {
        Key = "KugouMusicLyricRule";
        AppName = "酷狗音乐";
    }

    public override FoundWindow? DetectWindow()
    {
        var windows = Window.FindAllWindowsByClassName("kugou_ui", IntPtr.Zero)
            .Where(x => x.WindowName.Contains("桌面歌词 - 酷狗音乐"))
            .ToList();

        foreach (var window in windows)
        {
            _ = User32.GetWindowThreadProcessId(window.Handle, out var processId);

            try
            {
                using var process = Process.GetProcessById((int)processId);
                if (!process.ProcessName.Equals("KuGou", StringComparison.OrdinalIgnoreCase)) continue;
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