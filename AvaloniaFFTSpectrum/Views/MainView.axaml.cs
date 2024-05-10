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
    //  public int count_elements = 80;
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
        foreach (Border item in _stack.Children)
        {
            item.Background = new SolidColorBrush(_colorpicker.Color);
        }
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

                        for (int i = 0; i < jo.Length; i++)
                        {
                            if (i >= _stack.Children.Count)
                            {
                                _stack.Children.Add(new Border()
                                {
                                    Background = Brushes.Green,
                                    Width = DataContextVM.SizeScreen.Width / jo.Length,
                                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                                    Height = jo[i],

                                });
                                continue;
                            }

                            if (_stack.Children[i] is Border item)
                            {

                                item.Width = DataContextVM.SizeScreen.Width / jo.Length;
                                item.Height = jo[i];
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
