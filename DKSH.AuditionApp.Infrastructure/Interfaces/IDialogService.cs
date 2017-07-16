using System.Threading.Tasks;

namespace DKSH.AuditionApp.Infrastructure.Interfaces
{
    public interface IDialogService
    {
        Task<uint> SelectNumberDialog();
    }
}
