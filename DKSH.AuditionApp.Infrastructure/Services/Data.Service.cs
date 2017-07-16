using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Domain.Primitives;
using DKSH.AuditionApp.Infrastructure.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Infrastructure.Services
{
    /// <summary>
    /// Format representation of the business logic
    /// Encapsulates usage of data channel(s)
    /// </summary>
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private IChannelManager _channelManager;

        [ImportingConstructor]
        public DataService(IChannelManager channelManager)
        {
            _channelManager = channelManager;
        }

        public async Task<string> SendNumericData(uint num)
        {
            var result = await _channelManager.TrySend(BitConverter.GetBytes(num));
            return "result";
        }

        public async Task<bool> Signal()
        {
            var result = await _channelManager.TrySend(new byte[] { Convert.ToByte(Constants.SeriaPort.Signal_NumericData) });
            return result;
        }
    }
}
