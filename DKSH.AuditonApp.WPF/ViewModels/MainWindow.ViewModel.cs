using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Infrastructure.Interfaces;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Application.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> _signalState;
        public string SignalState => _signalState.Value;


        private string _numDataResponse;
        public string NumDataResponse {
            get { return _numDataResponse; }
            set {
                this.RaiseAndSetIfChanged(ref _numDataResponse, value);
                this.RaisePropertyChanged(nameof(NumDataResponse));
            }
        }

        public ReactiveCommand<Unit, Unit> SignalCommand { get; set; }

        private readonly IDataService _dataService;
        private readonly IChannelManager _channelManager;
        private readonly IDialogService _dialogService;

        public MainWindowViewModel(IDataService dataService, IChannelManager channelManager, IDialogService dialogService)
        {
            _dataService = dataService;
            _channelManager = channelManager;
            _dialogService = dialogService;

            // setup commands
            SignalCommand = ReactiveCommand.CreateFromTask(() => DoSignalAsync(), canExecute: _channelManager.IsConnected);

            // setup properties
            _channelManager.IsConnected.Select(isConnected => isConnected ? "Connected" : "Disconnected")
                                       .ToProperty(this, vm => vm.SignalState, out _signalState);
        }

        private async Task DoSignalAsync()
        {
            var result = await _dataService.Signal().ConfigureAwait(false);

            // bring user dialog to specify data
            var numData = await _dialogService.SelectNumberDialog().ConfigureAwait(false);

            // send data and update result on screen
            NumDataResponse = await _dataService.SendNumericData(numData).ConfigureAwait(false);
        }
    }
}
