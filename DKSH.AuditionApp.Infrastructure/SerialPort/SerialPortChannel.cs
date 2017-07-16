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
        private Thread _dedicatedThread;
        private ManualResetEvent _closing = new ManualResetEvent(false);

        public SerialPortChannel(string portName)
        {
            _portName = portName;
            _dedicatedThread = new Thread(new ThreadStart(PortMonitor))
            {
                IsBackground = false,
                Priority = ThreadPriority.Highest,
                Name = $"Thread-{portName}"
            };
            _dedicatedThread.TrySetApartmentState(ApartmentState.STA);

            ConfigrePort();
        }

        public override Task<bool> Connect()
        {
            _dedicatedThread.Start();
            return Task.FromResult(true);
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
            // leave bgr thread to cleanup
            _closing.Set();

            // await
            if (_dedicatedThread.IsAlive)
            {
                _dedicatedThread.Join(TimeSpan.FromSeconds(1));
            }
        }

        private void ConfigrePort()
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

        private void PortMonitor()
        {
            if (_serialPort.IsOpen && (base.State != ChannelState.Connected || base.State != ChannelState.Connected))
                return;

            try
            {
                // establish connection
                _serialPort.Open();
                base.State = ChannelState.Connecting;

                // request handshare
                Interlocked.Increment(ref _awaitingHandshake);
                _serialPort.Write(new char[] { Constants.SeriaPort.Handshake_Request }, 0, 1);

            }
            catch
            {
                base.State = ChannelState.Closed;
                return;
            }

            // keep thread alive for the lifetime of the port connection
            _closing.WaitOne();

            // cleanup
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardOutBuffer();
                    _serialPort.Close();
                }
                _serialPort.Dispose();
                _serialPort = null;
            }

            base.State = ChannelState.Closed;
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
