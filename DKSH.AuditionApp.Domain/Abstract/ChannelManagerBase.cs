using System;
using System.Threading.Tasks;
using DKSH.AuditionApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DKSH.AuditionApp.Domain.Abstract
{
    public abstract class ChannelManagerBase : IChannelManager
    {
        private readonly IEnumerable<IChannel> _channels;

        public ChannelManagerBase()
        {
            _channels = RetrieveChannels();
        }

        public async Task<bool> TryConnect()
        {
            if (_channels != null || !_channels.Any())
            {
                return false;
            }

            var connectTasks = _channels.ToList().Select(ch => ch.Connect());
            await Task.WhenAll(connectTasks);

            return true; //TODO: verify
        }

        public Task<bool> TrySend(byte[] data)
        {
            if (_channels != null || !_channels.Any())
            {
                return Task.FromResult(false);
            }

            var activeChannel = _channels.FirstOrDefault(ch => ch.State == Primitives.ChannelState.Connected);
            if (activeChannel != null)
            {
                return activeChannel.TrySend(data);
            }

            return Task.FromResult(false);
        }

        public async Task Disconnect()
        {
            var disconnectTasks = _channels.ToList().Select(ch => ch.Disconnect());
            await Task.WhenAll(disconnectTasks);
        }

        protected abstract IEnumerable<IChannel> RetrieveChannels();
    }
}
