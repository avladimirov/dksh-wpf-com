using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannelManager : IChannelManager
    {
        public bool CanSend {
            get {
                throw new NotImplementedException();
            }
        }

        public Task<bool> Send(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
