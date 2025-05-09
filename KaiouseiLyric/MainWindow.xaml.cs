using System.Windows;
using System.Windows.Interop;
using KaiouseiLyric.ViewModels;

namespace KaiouseiLyric;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
    }

    public static MainWindow Instance { get; private set; } = null!;

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel) return;
        viewModel.MainWindow = this;
        viewModel.MainWindowHandle = new WindowInteropHelper(this).Handle;
    }
}