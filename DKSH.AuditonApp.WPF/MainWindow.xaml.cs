using DKSH.AuditionApp.Application.ViewModels;
using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;

namespace DKSH.AuditionApp.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected MainWindowViewModel ViewModel { get; private set; }

        #region Services

        [Import]
        private IDataService dataService;

        [Import]
        private IChannelManager channelManager;

        [Import]
        private IDialogService dialogService;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            App.DIContainer.SatisfyImportsOnce(this);

            ViewModel = new MainWindowViewModel(dataService, channelManager, dialogService);
            DataContext = ViewModel;
        }


    }
}
