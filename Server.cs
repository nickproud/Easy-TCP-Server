using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyTCP
{
    public class Server
    {
        public bool Running;
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
                    try
                    {
                        Task.Run(() => new Channel(this).Open(client));
                    }
                    catch
                    {
                        //add handler here for adding to channel exception
                    }
                   
                }

            }
            catch(SocketException)
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
