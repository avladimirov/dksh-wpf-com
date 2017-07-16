using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Infrastructure.SerialPort;
using System.Threading.Tasks;
using System.Windows;

namespace DHSH_AuditonApp_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IChannelManager channelManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            channelManager = new SerialPortChannelManager();
            Task.Run(channelManager.TryConnect);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            channelManager.Disconnect();
        }
    }
}
