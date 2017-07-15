using DKSH.AuditionApp.Domain.Primitives;
using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface IChannel : ITransmit
    {
        event Action<ChannelState> StateChanged;

        ChannelState State { get; }

        Task<bool> Connect();

        Task<bool> Disconnect();
    }
}
