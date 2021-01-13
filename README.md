# Easy-TCP-Server
Easy-TCP-Server is an easy-to-implement TCP server which handles incoming TCP data from multiple clients via 'channels' of connections between the clients and the server. 

This project is built in .NET core as a class library for use in multi-platform projects requiring handling of TCP traffic. 

## Getting started

Clone this repo or download the source files and compile the project in visual studio. You can then reference EasyTCP.dll in your .NET Core Project.

To start a listening TCP server, you simply need to initialize an instance of the 'Server' class and then call the 'Start()' method. A server will listen for incoming TCP requests. In order to handle the incoming request, the 'Server' class has a 'DataReceived' class which you can subscribe to with an EventHandler that uses the usual 'object sender' parameter, along with 'DataReceivedArgs e' as the arguments parameter. 

Whenever an incoming TCP request is received by the server, a 'Channel' is opened. A 'Channel' represents a connection between server and client. Messages from the client are converted from bytes to a string in the 'Channel' and the 'DataReceived' event is fired. 

## Example Implementation in a .NET Core Console App. 

The below is a demonstration of a simple console application which spins up a server and handles incoming requests

```csharp
class Program
    {
        public static Server TCPServer = new Server();

        static void Main(string[] args)
        {
            TCPServer.Start();
            TCPServer.DataReceived += server_OnDataIn;
            Console.WriteLine("Server running");
            Console.ReadLine();
        }

        public static void server_OnDataIn(object sender, DataReceivedArgs e)
        {
            Console.WriteLine(e.Message);
            if(e.Message == "HELLO")
            {
                e.ThisChannel.Send("MESSAGE RECEIVED");
            }
            if(e.Message == "CLOSE")
            {
                e.ThisChannel.Close();
            }
        }
    }
    
    ```


