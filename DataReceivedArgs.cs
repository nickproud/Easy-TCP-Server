
using System;
using System.Linq;

namespace EasyTCP
{
    
    public class DataReceivedArgs : EventArgs, IDisposable
    {
        public string ConnectionId { get; set; }
        public string Message { get; set; }
        public Channel ThisChannel { get; set; }

        public void Dispose()
        {
            ((IDisposable)ThisChannel).Dispose();
        }
    }
}
