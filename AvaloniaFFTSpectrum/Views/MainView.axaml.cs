using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaFFTSpectrum.ViewModels;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AvaloniaFFTSpectrum.Views;

public partial class MainView : UserControl
{
    private UdpClient Client = new UdpClient()
    {
        EnableBroadcast = true
    };
    SolidColorBrush solidColorBrush = new SolidColorBrush(Brushes.Green.Color);
    public ViewModel__MainWindow DataContextVM => this.DataContext as ViewModel__MainWindow;
    public MainView()
    {
        InitializeComponent();
        this.DataContext = new ViewModel__MainWindow();
        this.Loaded += MainView_Loaded;

        _grid.SizeChanged += _grid_SizeChanged;
        _colorpicker.PointerMoved += _colorpicker_PointerMoved;

        _button_color.Click += (o, e) =>
        {
            _colorpicker.IsVisible = !_colorpicker.IsVisible;
        };
    }

    private void _colorpicker_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {

        solidColorBrush = new SolidColorBrush(_colorpicker.Color);
    }

    private void _grid_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (this.DataContext is ViewModel__MainWindow vmodel)
        {
            vmodel.SizeScreen = e.NewSize;

        }
    }

    private void MainView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        new Thread(() =>
        {
            while (true)
            {
                try
                {
                    Client.Send(new byte[] { 0, 0, 0, 0 }, 0, new IPEndPoint(IPAddress.Broadcast, 7070));
                    byte[] jo = Client.ReceiveAsync().Result.Buffer;
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        double w_ = _grid_items.Bounds.Width / jo.Length;
                        for (int i = 0; i < jo.Length; i++)
                        {
                            if (i >= _grid_items.Children.Count)
                            {
                                _grid_items.Children.Add(new Border()
                                {
                                    Background = solidColorBrush,
                                    Width = w_,
                                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                                    Height = _grid_items.Height / 255 * jo[i],
                                    Margin = new Avalonia.Thickness(w_ * i, 0)
                                });
                                continue;
                            }

                            if (_grid_items.Children[i] is Border item)
                            {

                                item.Width = w_;
                                item.Height = _grid_items.Bounds.Height / 255 * jo[i];
                                item.Margin = new Avalonia.Thickness(w_ * i, 0);
                                item.Background = solidColorBrush;
                            }
                        }
                    });

                    Thread.Sleep(1000 / 20);

                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Thread.Sleep(500);
                }

            }
        }).Start();
    }
}
