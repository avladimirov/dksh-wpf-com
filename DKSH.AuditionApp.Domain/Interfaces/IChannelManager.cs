using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface IChannelManager : ITransmit
    {
        IObservable<bool> IsConnected { get; }

        Task<bool> TryConnect();

        Task Disconnect();
    }
}
