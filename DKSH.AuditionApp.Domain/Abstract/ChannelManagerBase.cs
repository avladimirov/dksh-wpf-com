using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Abstract
{
    public abstract class ChannelManagerBase : IChannelManager
    {
        protected IEnumerable<IChannel> Channels { get; private set; }

        private readonly BehaviorSubject<bool> _isConnected = new BehaviorSubject<bool>(false);
        public IObservable<bool> IsConnected { get { return _isConnected; } } 

        public ChannelManagerBase()
        {
            // setup channels
            Channels = RetrieveChannels();

            // wire status updates
            if (Channels == null || !Channels.Any()) return;

            // monitor all channels, optimize manager status update
            var chMonitor = Channels.Select(ch => Observable.FromEvent<Action<ChannelState>, IChannel>(h => ch.StateChanged += h, h => ch.StateChanged -= h));
            Observable.Concat(chMonitor)
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .Subscribe((ch) =>
                      {
                          var hasAtLeastOneConnected = Channels.Any(c => c.State == ChannelState.Connected);
                          _isConnected.OnNext(hasAtLeastOneConnected);
                      });
        }

        public async Task<bool> TryConnect()
        {
            if (Channels == null || !Channels.Any())
            {
                return false;
            }

            var connectTasks = Channels.ToList().Select(ch => ch.Connect());
            await Task.WhenAll(connectTasks);

            return true; //TODO: verify
        }

        public Task<bool> TrySend(byte[] data)
        {
            if (Channels != null || !Channels.Any())
            {
                return Task.FromResult(false);
            }

            var activeChannel = Channels.FirstOrDefault(ch => ch.State == Primitives.ChannelState.Connected);
            if (activeChannel != null)
            {
                return activeChannel.TrySend(data);
            }

            return Task.FromResult(false);
        }

        public async Task Disconnect()
        {
            var disconnectTasks = Channels.ToList().Select(ch => ch.Disconnect());
            await Task.WhenAll(disconnectTasks);
        }

        protected abstract IEnumerable<IChannel> RetrieveChannels();
    }
}
