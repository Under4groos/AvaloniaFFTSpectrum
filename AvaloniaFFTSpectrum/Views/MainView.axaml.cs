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

    public MainView()
    {
        InitializeComponent();
        this.DataContext = new ViewModel__MainWindow();
        this.Loaded += MainView_Loaded;

        _grid.SizeChanged += _grid_SizeChanged;
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
        if (this.DataContext is ViewModel__MainWindow vmodel)
        {

            for (int i = 0; i < 40; i++)
            {
                _stack.Children.Add(new Border()
                {
                    Background = Brushes.Green,
                    Width = vmodel.SizeScreen.Width / 40,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                    Height = 1,

                });
                //var vv_ = new ViewModels.Panel()
                //{
                //    Height = 2,
                //    Width = vmodel.SizeScreen.Width / 40,

                //};
                //vmodel.FFT.Add(vv_);
            }

        }

        new Thread(() =>
        {


            while (true)
            {
                try
                {
                    Client.Send(new byte[] { }, 0, new IPEndPoint(IPAddress.Broadcast, 7070));



                    //string g = Encoding.UTF8.GetString(Client.ReceiveAsync().Result.Buffer);
                    //var jo = JsonConvert.DeserializeObject<ObservableCollection<int>>(g);



                    byte[] jo = Client.ReceiveAsync().Result.Buffer;


                    Dispatcher.UIThread.Invoke(() =>
                    {
                        if (this.DataContext is ViewModel__MainWindow vmodel)
                        {
                            for (int i = 0; i < jo.Length; i++)
                            {
                                //vmodel.FFT[i] = new ViewModels.Panel()
                                //{

                                //    Width = vmodel.SizeScreen.Width / jo.Length,
                                //    Height = jo[i]
                                //};

                                if (_stack.Children[i] is Border item)
                                {

                                    item.Width = vmodel.SizeScreen.Width / jo.Length;
                                    item.Height = jo[i];
                                }


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
