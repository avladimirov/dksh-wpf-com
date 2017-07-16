using DKSH.AuditionApp.Infrastructure.SerialPort;
using System.Windows;

namespace DKSH.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var manager = new SerialPortChannelManager();

            manager.TryConnect();
        }
    }
}
