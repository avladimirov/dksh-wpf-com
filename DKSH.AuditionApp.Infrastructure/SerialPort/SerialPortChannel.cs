using DKSH.AuditionApp.Domain.Abstract;
using DKSH.AuditionApp.Domain.Primitives;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ports = System.IO.Ports;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannel : ChannelBase, IDisposable
    {
        private readonly string _portName;
        private Ports.SerialPort _serialPort;
        private int _awaitingHandshake;

        public SerialPortChannel(string portName)
        {
            _portName = portName;
            SetupPort();
        }

        public override Task<bool> Connect()
        {
            if (_serialPort.IsOpen && (base.State != ChannelState.Connected || base.State != ChannelState.Connected))
                return Task.FromResult(false);

            return Task.Run(() =>
            {
                try
                { 
                    // establish connection
                    _serialPort.Open();
                    base.State = ChannelState.Connecting;

                    // request handshare
                    Interlocked.Increment(ref _awaitingHandshake);
                    _serialPort.Write(new char[] { Constants.SeriaPort.Handshake_Request }, 0, 1);

                    return true; //TODO: await handle on hanshare response
                }
                catch
                {
                    base.State = ChannelState.Disconected;

                }
                return true;
            });
        }

        public override Task<bool> TrySend(byte[] data)
        {
            if (State != ChannelState.Connected)
            {
                return Task.FromResult(false);
            }

            return Task.Run(() =>
            {
                _serialPort.Write(data, 0, data.Length);
            }).ContinueWith<bool>(t =>
            {
                return t.IsCompleted;
            });
            //TODO: add continuations for failure and success
        }

        public override Task<bool> Disconnect()
        {
            if (State != ChannelState.Connected)
            {
                return Task.FromResult(false);
            }

            base.State = ChannelState.Disconected;
            return Task.FromResult(true);
        }

        public void Dispose()
        {
            if(_serialPort != null)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        private void SetupPort()
        {
            // safe initialize
            _serialPort = new Ports.SerialPort();

            // configure
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 38400;
            _serialPort.Handshake = Ports.Handshake.None;
            _serialPort.Parity = Ports.Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = Ports.StopBits.One;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 50;
            _serialPort.Encoding = Encoding.ASCII;

            // wire handlers
            _serialPort.DataReceived += (s, args) =>
            {
                switch (args.EventType)
                {
                    case Ports.SerialData.Chars: ProcessChars(); break;
                    case Ports.SerialData.Eof: ProcessBuffer(); break;
                }
            };
            _serialPort.ErrorReceived += (s, args) =>
            {
                //TODO: log
            };
            _serialPort.Disposed += (s, args) =>
            {
                Dispose();
                //SetupCom();
            };
        }

        private void ProcessChars()
        {
            var charData = _serialPort.ReadChar();
            // 
            if (_awaitingHandshake > 0 && Convert.ToChar(charData) == Constants.SeriaPort.Handshake_Response)
            {
                base.State = ChannelState.Connected;
                Interlocked.Decrement(ref _awaitingHandshake);
            }
        }

        private void ProcessBuffer()
        {
            //throw new NotImplementedException();
        }

    }
}
