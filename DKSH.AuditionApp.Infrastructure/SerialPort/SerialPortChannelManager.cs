using DKSH.AuditionApp.Domain.Abstract;
using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management;
using Ports = System.IO.Ports;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    [Export(typeof(IChannelManager))]
    public class SerialPortChannelManager : ChannelManagerBase, IDisposable
    {
        protected override IEnumerable<IChannel> RetrieveChannels()
        {
            var availablePorts = GetSerialPortsManaged() ?? GetSerialPortsWMI();
            if (availablePorts != null && availablePorts.Any())
            {
                return availablePorts.Select(port => new SerialPortChannel(port));
            }

            return null;
        }

        private IEnumerable<string> GetSerialPortsManaged()
        {
            try
            {
                return Ports.SerialPort.GetPortNames();
            }
            catch { }
            return null;
        }

        private IEnumerable<string> GetSerialPortsWMI()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                var ports = searcher.Get().OfType<ManagementBaseObject>();
                return ports?.Select(p => p["DeviceID"] as string);
            }
        }

        public void Dispose()
        {
            var disposableChannels = Channels.OfType<IDisposable>().ToList();
            if (disposableChannels.Any())
            {
                disposableChannels.ForEach(ch => ch.Dispose());
            }
        }
    }
}
