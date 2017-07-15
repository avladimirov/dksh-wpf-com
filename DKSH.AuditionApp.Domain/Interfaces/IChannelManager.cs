using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface IChannelManager : ITransmit
    {
        Task<bool> TryConnect();

        Task Disconnect();
    }
}
