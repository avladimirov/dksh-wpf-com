using System.Threading.Tasks;

namespace DKSH.AuditionApp.Infrastructure.Interfaces
{
    public interface IDataService 
    {
        Task<bool> Signal();

        Task<string> SendNumericData(uint num);
    }
}
