using DKSH.AuditionApp.Domain.Abstract;
using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Tests.Fakes
{
    internal class TestChannel : ChannelBase
    {
        private readonly string _name;

        public TestChannel(string name)
        {
            _name = name;
        }

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
