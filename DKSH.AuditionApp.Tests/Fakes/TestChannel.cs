using DKSH.AuditionApp.Domain.Abstract;
using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Tests.Fakes
{
    internal class TestChannel : ChannelBase
    {
        public override Task<bool> Connect()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Disconnect()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> TrySend(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
