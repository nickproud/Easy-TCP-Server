using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyTCP
{
    public class Channel
    {
        public readonly Server ThisServer;
        private TcpClient ThisClient;
        private byte[] buffer;
        private NetworkStream stream;
        private bool isOpen;
        public Channel(Server myServer)
        {
            ThisServer = myServer;
            buffer = new byte[256];
        }

        public void Open(TcpClient client)
        {
            ThisClient = client;
            isOpen = true;
            string data = "";
            using (stream = ThisClient.GetStream())
            {
                int position;
                try
                {
                    while ((position = stream.Read(buffer, 0, buffer.Length)) != 0 && isOpen)
                    {
                        data = Encoding.UTF8.GetString(buffer, 0, position);
                        var args = new DataReceivedArgs()
                        {
                            Message = data,
                            ConnectionId = 12345,
                            ThisChannel = this
                        };

                        ThisServer.OnDataIn(args);
                    }

                }
                catch (ObjectDisposedException)
                {
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
            stream.Dispose();
            ThisClient.Close();
            
        }

    }
}
