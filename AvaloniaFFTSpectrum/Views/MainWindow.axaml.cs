using Avalonia.Controls;
using System.Diagnostics;

namespace AvaloniaFFTSpectrum.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closed += MainWindow_Closed;
        this.Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void MainWindow_Closed(object? sender, System.EventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }
}
