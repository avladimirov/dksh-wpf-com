using DKSH.AuditionApp.Domain.Primitives;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Tests.VirtualSerialPort
{
    internal static class PortSimulator
    {
        private static SerialPort _port;
        private static bool _signalNumericData;

        internal static bool SetupPort(string portName)
        {
            try
            {
                _port = new SerialPort(portName);
                _port.DataReceived += (s, args) =>
                {
                    var strData = _port.ReadExisting();
                    if (strData == null) return;

                    // process by simulating specification
                    EvaluateHandshake(strData);
                    EvaluateSignal(strData);
                };
                _port.Open();

                return true;
            }
            catch (Exception ex)
            {

                //TODO: log
            }

            return false;
        }

        private static void EvaluateHandshake(string strData)
        {
            if (strData.Length == 1 && strData.First() == Constants.SeriaPort.Handshake_Request)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    _port.Write(new char[] { Constants.SeriaPort.Handshake_Response }, 0, 1);
                });
            }
        }

        private static void EvaluateSignal(string strData)
        {
            if (strData.Length == 1 && strData.First() == Constants.SeriaPort.Signal_NumericData)
            {
                _signalNumericData = true;
            }
        }
    }
}
