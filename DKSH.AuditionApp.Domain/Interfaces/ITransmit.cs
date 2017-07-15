using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface ITransmit
    {
        Task<bool> TrySend(byte[] data);
    }
}
