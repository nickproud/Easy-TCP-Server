using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EasyTCP
{
  
    public class Server
    {
        private bool _running;
        public bool Running
        {
            get
            {
                return _running;
            }
            set
            {
                _running = value;
            }
        }
        public event EventHandler<DataReceivedArgs> DataReceived;
        private TcpListener Listener;
        public Channels ConnectedChannels;
        
        public Server()
        {
            Listener = new TcpListener(IPAddress.Parse(Globals.ServerAddress), Globals.ServerPort);
        }

        public async void Start()
        {
            try
            {
                Listener.Start();
                Running = true;
                ConnectedChannels = new Channels(this);
                while (Running)
                {
                    var client = await Listener.AcceptTcpClientAsync();
                    Task.Run(() => new Channel(this).Open(client));
                }

            }
            catch(SocketException)
            {
                throw;
            }
            catch(ChannelRegistrationException)
            {
                throw;
            }
        }

        public void Stop()
        {
            Listener.Stop();
            Running = false;
        }

        public void OnDataIn(DataReceivedArgs e)
        {
            DataReceived?.Invoke(this, e);
        }
    }
}
