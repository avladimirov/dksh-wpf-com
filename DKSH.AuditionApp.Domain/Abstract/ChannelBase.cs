using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Domain.Primitives;
using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Abstract
{
    public abstract class ChannelBase : IChannel
    {
        private ChannelState _state;
        public ChannelState State {
            get { return _state; }
            set {
                if(_state != value)
                {
                    _state = value;
                    StateChanged?.Invoke(_state);
                }
            }
        }

        public event Action<ChannelState> StateChanged;

        public abstract Task<bool> Connect();

        public abstract Task<bool> Disconnect();

        public abstract Task<bool> TrySend(byte[] data);
    }
}
