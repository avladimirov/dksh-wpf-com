using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DKSH.AuditionApp.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private CompositionContainer diContainer;
        private IChannelManager _channelManager;

        public App()
        {
            this.InitializeContainter();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _channelManager = diContainer.GetExport<IChannelManager>().Value;
            Task.Run(_channelManager.TryConnect);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _channelManager.Disconnect();
        }

        private void InitializeContainter()
        {
            var catalogs = new AggregateCatalog();
            AppDomain.CurrentDomain.GetAssemblies().ToList()
                                   .ForEach(asm => catalogs.Catalogs.Add(new AssemblyCatalog(asm)));

            diContainer = new CompositionContainer(catalogs, CompositionOptions.ExportCompositionService | CompositionOptions.IsThreadSafe);
        }
    }
}
