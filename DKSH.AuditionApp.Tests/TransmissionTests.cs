using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DKSH.AuditionApp.Tests
{
    [TestClass]
    public class TransmissionTests
    {
        private IChannelManager channelManager;

        [TestInitialize]
        public void Setup()
        {
            channelManager = new TestChannelManager();
        }

        [TestMethod]
        public void EstablishConnectionTest()
        {
        }
    }
}
