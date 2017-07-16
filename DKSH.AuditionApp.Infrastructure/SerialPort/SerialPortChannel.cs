using DKSH.AuditionApp.Domain.Abstract;
using System;
using System.Threading.Tasks;
using Ports = System.IO.Ports;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannel : ChannelBase, IDisposable
    {
        private static readonly char RequestHandshake = 'i';
        private static readonly char ResponseHandshake = 'c';

        private readonly string _portName;
        private Ports.SerialPort _serialPort;
        private bool _awaitingConnResponse;

        public SerialPortChannel(string portName)
        {
            this._portName = portName;
            this.SetupCom();
        }

        public override Task<bool> Connect()
        {
            if (this._serialPort.IsOpen && (base.State != Domain.Primitives.ChannelState.Connected || base.State != Domain.Primitives.ChannelState.Connected))
                return Task.FromResult(false);

            return Task.Run(() =>
            {
                try
                { 
                    // establish connection
                    this._serialPort.Open();
                    // request handshare
                    this._serialPort.Write(new char[] { RequestHandshake }, 0, 1);

                    return true; //TODO: await handle on hanshare response
                }
                catch
                {
                    base.State = Domain.Primitives.ChannelState.Disconected;

                }
                return true;
            });
        }

        public override Task<bool> TrySend(byte[] data)
        {
            if (this.State != Domain.Primitives.ChannelState.Connected)
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
            if (this.State != Domain.Primitives.ChannelState.Connected)
            {
                return Task.FromResult(false);
            }

            base.State = Domain.Primitives.ChannelState.Disconected;
            return Task.FromResult(true);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void SetupCom()
        {
            // configure
            this._serialPort.PortName = _portName;
            this._serialPort.BaudRate = 38400;
            this._serialPort.Handshake = Ports.Handshake.None;
            this._serialPort.Parity = Ports.Parity.None;
            this._serialPort.DataBits = 8;
            this._serialPort.StopBits = Ports.StopBits.One;
            this._serialPort.ReadTimeout = 500;
            this._serialPort.WriteTimeout = 50;

            // wire handlers
            this._serialPort.DataReceived += (s, args) =>
            {
                switch (args.EventType)
                {
                    case Ports.SerialData.Chars: ProcessChars(); break;
                    case Ports.SerialData.Eof: ProcessBuffer(); break;
                }
            };
            this._serialPort.ErrorReceived += (s, args) =>
            {
                //TODO: log
            };
            this._serialPort.Disposed += (s, args) =>
            {
                Dispose();
                //SetupCom();
            };
        }

        private void ProcessChars()
        {
            var charData = this._serialPort.ReadChar();
            // 
            if (_awaitingConnResponse && Convert.ToChar(charData) == ResponseHandshake)
            {
                base.State = Domain.Primitives.ChannelState.Connected;
            }
        }

        private void ProcessBuffer()
        {
            //throw new NotImplementedException();
        }


    }
}
