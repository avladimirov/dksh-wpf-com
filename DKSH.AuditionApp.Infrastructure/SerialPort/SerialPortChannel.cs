using DKSH.AuditionApp.Domain.Abstract;
using System;
using System.Threading.Tasks;
using Ports = System.IO.Ports;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannel : ChannelBase, IDisposable
    {
        private readonly string _portName;
        private readonly Ports.SerialPort _serialPort;

        public SerialPortChannel(string portName)
        {
            this._portName = portName;
            // safe initialize
            this._serialPort = new System.IO.Ports.SerialPort();
        }

        public override Task<bool> Connect()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> TrySend(byte[] data)
        {
            if(State == Domain.Primitives.ChannelState.Connected)
            {
                return Task.Run(() =>
                {
                    _serialPort.Write(data, 0, data.Length);
                }).ContinueWith<bool>(t =>
                {
                    return t.IsCompleted;
                });
                //TODO: add continuations for failure and success
            }

            return Task.FromResult(false);
        }

        public override Task<bool> Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Configure()
        {
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 38400;
            _serialPort.Handshake = Ports.Handshake.None;
            _serialPort.Parity = Ports.Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = Ports.StopBits.One;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 50;
        }
    }
}
