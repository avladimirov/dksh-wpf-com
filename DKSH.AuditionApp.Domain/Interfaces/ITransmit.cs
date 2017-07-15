using System.Threading.Tasks;

namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface ITransmit
    {
        Task<bool> Send(byte[] data);
    }
}
