﻿using DKSH.AuditionApp.Domain.Abstract;
using DKSH.AuditionApp.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DKSH.AuditionApp.Infrastructure.SerialPort
{
    public class SerialPortChannelManager : ChannelManagerBase
    {
        protected override IEnumerable<IChannel> RetrieveChannels()
        {
            throw new NotImplementedException();
        }
    }
}
