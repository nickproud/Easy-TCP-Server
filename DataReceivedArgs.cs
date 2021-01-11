using System;
using System.Collections.Generic;
using System.Text;

namespace EasyTCP
{
    public class DataReceivedArgs : EventArgs
    {
        public long ConnectionId { get; set; }
        public string Message { get; set; }
        public Channel ThisChannel { get; set; }
    }
}
