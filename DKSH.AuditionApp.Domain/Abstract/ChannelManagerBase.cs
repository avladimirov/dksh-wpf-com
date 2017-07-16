using DKSH.AuditionApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Abstract
{
    public abstract class ChannelManagerBase : IChannelManager
    {
        protected IEnumerable<IChannel> Channels { get; private set; }

        public ChannelManagerBase()
        {
            Channels = RetrieveChannels();
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
