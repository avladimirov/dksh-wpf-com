﻿using System;
using System.Threading.Tasks;
using DKSH.AuditionApp.Domain.Interfaces;
using DKSH.AuditionApp.Domain.Primitives;
using DKSH.AuditionApp.Domain.Abstract;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannel : ChannelBase
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
