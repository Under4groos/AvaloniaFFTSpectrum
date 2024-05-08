using Avalonia;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvaloniaFFTSpectrum.ViewModels
{
    public struct Panel
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }
    public class ViewModel__MainWindow : ViewModelBase
    {
        private ObservableCollection<Panel> _FFT = new ObservableCollection<Panel>();

        public ObservableCollection<Panel> FFT
        {
            get => _FFT;
            set => this.RaiseAndSetIfChanged(ref _FFT, value);
        }



        private Size _SizeScreen = new Size();
        public Size SizeScreen
        {
            get => _SizeScreen;
            set => this.RaiseAndSetIfChanged(ref _SizeScreen, value);
        }


        private double _WidthPanel = 0;
        public double WidthtPanel
        {
            get => _WidthPanel;
            set => this.RaiseAndSetIfChanged(ref _WidthPanel, value);
        }




    }
}
