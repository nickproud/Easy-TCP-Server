
using System.Collections.Concurrent;

namespace EasyTCP
{
    public class Channels
    {
        public ConcurrentDictionary<string, Channel> OpenChannels;
        private readonly Server thisServer;
        //test comment
        public Channels(Server myServer)
        {
            OpenChannels = new ConcurrentDictionary<string, Channel>();
            thisServer = myServer;
        }
    }
}
