using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyTCP
{
    public class Channel
    {
        public readonly Server ThisServer;

        public Channel(Server myServer)
        {
            ThisServer = myServer;
        }

        public void Open(TcpClient client)
        {
            var buffer = new byte[256];
            string data = "";
            var stream = client.GetStream();
            int position = 0;
            while ((position = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                data = System.Text.Encoding.UTF8.GetString(buffer, 0, position);
                var args = new DataReceivedArgs()
                {
                    Message = data,
                    ConnectionId = 12345
                };

                ThisServer.OnDataIn(args);

            }
           
        }

    }
}
