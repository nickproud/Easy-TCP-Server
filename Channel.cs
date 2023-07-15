using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyTCP
{
    public class Channel : IDisposable
    {
        private Server thisServer;
        public readonly string Id;
        private TcpClient thisClient;
        private readonly byte[] buffer;
        private NetworkStream stream;
        private bool isOpen;
        private bool disposed;

        public Channel(Server myServer)
        {
            thisServer = myServer;
            buffer = new byte[256];
            Id = Guid.NewGuid().ToString();
        }

        public void Open(TcpClient client)
        {
            thisClient = client;
            isOpen = true;
            if(!thisServer.ConnectedChannels.OpenChannels.TryAdd(Id, this))
            {
                isOpen = false;
                throw (new ChannelRegistrationException("Unable to add channel to channel list"));
            }
            string data = "";
            using (stream = thisClient.GetStream())
            {
                int position;

                while(isOpen)
                {
                    if (clientDisconnected())
                    {
                        Close();
                    }
                    else
                    {
                        while ((position = stream.Read(buffer, 0, buffer.Length)) != 0 && isOpen)
                        {
                            data = Encoding.UTF8.GetString(buffer, 0, position);
                            var args = new DataReceivedArgs()
                            {
                                Message = data,
                                ConnectionId = Id,
                                ThisChannel = this
                            };

                            thisServer.OnDataIn(args);
                            if(!isOpen) { break; }
                        }
                    }
                }
            }
        }

        public void Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }


        public void Close()
        {
            Dispose(false);
            isOpen = false;
            thisServer.ConnectedChannels.OpenChannels.TryRemove(Id, out Channel removedChannel);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                stream.Close();
                thisClient.Close();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool clientDisconnected()
        {
            return (thisClient.Client.Available == 0 && thisClient.Client.Poll(1, SelectMode.SelectRead));
        }
    }
}
