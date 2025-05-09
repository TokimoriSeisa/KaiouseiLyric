using CommunityToolkit.Mvvm.ComponentModel;
using KaiouseiLyric.Service;

namespace KaiouseiLyric.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private MainWindow _mainWindow = null!;
    [ObservableProperty] private IntPtr _mainWindowHandle;
    [ObservableProperty] private WindowManager _manager = WindowManager.Instance;
}