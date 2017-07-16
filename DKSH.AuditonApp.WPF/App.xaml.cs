using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DKSH.AuditionApp.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static CompositionContainer DIContainer { get; private set; }

        private IChannelManager _channelManager;

        public App()
        {
            DIContainer = new CompositionContainer(new ApplicationCatalog(), CompositionOptions.ExportCompositionService | CompositionOptions.IsThreadSafe);
            _channelManager = DIContainer.GetExport<IChannelManager>().Value;

            // listen for unhandled exceptions
            Current.DispatcherUnhandledException += Dispatcher_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Task.Run(_channelManager.TryConnect);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _channelManager.Disconnect();

            // cleanup
            var disposable = _channelManager as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
        }

        #region Exception handling

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }

        #endregion

    }
}
