# Easy-TCP-Server
Easy-TCP-Server is an easy-to-implement TCP server which handles incoming TCP data from multiple clients via 'channels' of connections between the clients and the server. 

This project is built in .NET core as a class library for use in multi-platform projects requiring handling of TCP traffic. 

## Getting started

Clone this repo or download the source files and compile the project in visual studio. You can then reference EasyTCP.dll in your .NET Core Project.

To start a listening TCP server, you simply need to initialize an instance of the 'Server' class and then call the 'Start()' method. A server will listen for incoming TCP requests. In order to handle the incoming request, the 'Server' class has a 'DataReceived' class which you can subscribe to with an EventHandler that uses the usual 'object sender' parameter, along with 'DataReceivedArgs e' as the arguments parameter. 

Whenever an incoming TCP request is received by the server, a 'Channel' is opened. A 'Channel' represents a connection between server and client. Messages from the client are converted from bytes to a string in the 'Channel' and the 'DataReceived' event is fired. 

The server works asynchronously firing a Task to handle each incoming client, and as a result can handle multiple clients simultaneously.

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
The program above starts a server and then subscribes our 'server_OnDataIn' method to the server's 'DataReceived' event. 
When the event fires, we are able to access properties concerning the incoming TCP data in the 'DataReceivedArgs.'
Each instance of 'DataReceivedArgs' contains the channel on which the data was received. As you can see in the example above, we are checking the contents of the incoming message. If the message is equal to "CLOSE", we are using the 'Close()' method on the current channel to close the client's connection to the server. 


## Ports and Host Addresses
By default, any server will listen on the localhost IP (127.0.0.1) on port 12400. If you want to change this for any server instance, you can do so in the static 'Globals' class. Otherwise, you can use the overloaded constructor for the 'Server' that passes the IP and Port explicitly. 
    
### Change IP and Port globally
    
```csharp
    static class Globals
    {
        public const int ServerPort = 12400;
        public const string ServerAddress = "127.0.0.1";
    }
```
    
### Explicitly define an IP and Port on server creation
```csharp
    var server = new Server(myIP, myPort);
```
### Manage connected clients

Any client which connects to the server is stored in a concurrent dictionary called 'OpenChannels' found in the 'Channels' class, an instance of which is created within each 'Server' instance. Each client can be accessed in the dictionary using the client ConnectionID property as a key. 
    
    
    
    


