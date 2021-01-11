using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EasyTCP
{
    public class Channels
    {
        public ConcurrentDictionary<string, Channel> OpenChannels;
        private Server ThisServer;
        public Channels(Server myServer)
        {
            OpenChannels = new ConcurrentDictionary<string, Channel>();
            ThisServer = myServer;
        }
    }
}
