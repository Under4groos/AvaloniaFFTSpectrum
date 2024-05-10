
using FFT_SpecrtumLib.Enums;
using FFT_SpecrtumLib.Model;
using FFT_SpecrtumLib.ViewModels;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

{
    UdpClient Server = new UdpClient(7070);
    IPEndPoint ClientEp = new IPEndPoint(IPAddress.Any, 0);

    FFTAnalyzer viewModel_Analyzer = new FFTAnalyzer()
    {
        ScalingStrategy = ScalingStrategy.Linear,
        Rate = 60,
    };
    byte[] data = { };
    new Thread(() =>
    {
        while (true)
        {
            try
            {
                byte[] bytes_ = Server.Receive(ref ClientEp);
                //if ((from b in bytes_ where b == 0 select b).Count() == 0)
                //{
                //    Server.Send(data, data.Length, ClientEp);
                //}
                Server.Send(data, data.Length, ClientEp);
            }
            catch (Exception)
            {


            }
        }


    }).Start();


    viewModel_Analyzer.event_fftreturn = (ObservableCollection<ViewModel_FrequencyBin> FrequencyBins) =>
    {
        try
        {

            var ar = (from byte_ in FrequencyBins select (int)byte_.Value).ToList().GetRange(0, 40).ToArray();

            data = (from b in ar select (byte)b).ToArray();
            Console.WriteLine(string.Join(" ", ar));


        }
        catch (Exception e)
        {

            Console.WriteLine(e.Message);
        }
    };

}