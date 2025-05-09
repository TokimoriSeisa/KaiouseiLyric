using System.Text;
using Vanara.PInvoke;

namespace KaiouseiLyric.Shared.Utils;

public class Window
{
    public static IList<FoundWindow> FindAllWindows(string windowName, HWND parent)
    {
        var result = new List<FoundWindow>();

        windowName = ParseGuidToString(windowName);

        var hWnd = HWND.NULL;
        do
        {
            if ((hWnd = User32.FindWindowEx(parent, hWnd, null, windowName)) != HWND.NULL)
            {
                var window = new FoundWindow
                {
                    Handle = hWnd,
                    WindowName = windowName
                };

                var className = new StringBuilder(255);
                User32.GetClassName(hWnd, className, className.Capacity);
                window.ClassName = className.ToString();

                result.Add(window);
            }
        } while (hWnd != IntPtr.Zero);

        return result;
    }

    public static IList<FoundWindow> FindAllWindowsByClassName(string className, HWND parent)
    {
        var result = new List<FoundWindow>();
        var hWnd = HWND.NULL;
        do
        {
            if ((hWnd = User32.FindWindowEx(parent, hWnd, className)) != HWND.NULL)
            {
                var window = new FoundWindow
                {
                    Handle = hWnd,
                    WindowName = GetWindowTitle(hWnd),
                    ClassName = className
                };

                result.Add(window);
            }
        } while (hWnd != IntPtr.Zero);

        return result;
    }

    private static string ParseGuidToString(string guid)
    {
        if (guid.Length == 38 && guid.Contains('-'))
        {
            var s = guid.Replace("-", "");
            var b = Enumerable.Range(0, s.Length).Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(s.Substring(x, 2), 16)).ToArray();

            byte G(int v)
            {
                return (byte)((v * 1103515245 + 12345) & 0x7fffffff);
            }

            var e = b[^1];

            for (var i = 0; i < b.Length; ++i)
                b[i] ^= e = G(e);

            guid = Encoding.UTF8.GetString(b);
            var index = guid.IndexOf('\0');
            guid = guid.Substring(0, index);
        }

        return guid;
    }

    public static string GetWindowTitle(HWND hWnd)
    {
        var length = User32.GetWindowTextLength(hWnd) + 1;
        var title = new StringBuilder(length);
        _ = User32.GetWindowText(hWnd, title, length);
        return title.ToString();
    }
}

/// <summary>
///     Describes a window found with the FindAllWindows function.
/// </summary>
public class FoundWindow
{
    public HWND Handle { get; set; } = HWND.NULL;
    public string ClassName { get; set; } = string.Empty;
    public string WindowName { get; set; } = string.Empty;

    public override string ToString()
    {
        return ClassName;
    }
}