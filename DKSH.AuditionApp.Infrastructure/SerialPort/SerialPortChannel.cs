using System;
using System.Threading.Tasks;
using DKSH.AuditionApp.Domain.Interfaces;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannel : IChannel
    {
        public bool IsActive {
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
