using DKSH.AuditionApp.Domain.Abstract;
using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DKSH.AuditionApp.Tests.Fakes
{
    internal class TestChannelManager : ChannelManagerBase
    {
        protected override IEnumerable<IChannel> RetrieveChannels()
        {
            yield return new TestChannel("COM1");
            yield return new TestChannel("COM2");
            yield return new TestChannel("COM3");
        }
    }
}
