namespace cshttp;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class ServerOption
{
    public required String host { get; set; }
    public required Int32 port { get; set; }
    public String? staticPath { get; set; }
}

public class Server
{
    ServerOption options;

    Server(ServerOption options)
    {
        this.options = options;
    }

    public void start()
    {
        IPAddress addr = IPAddress.Parse(options.host);
        TcpListener server = new TcpListener(addr, options.port);

        server.Start();

        while (true)
        {
            using TcpClient client = server.AcceptTcpClient();
        }
    }
    void handleRequest(TcpClient client)
    { // TODO: process client request
    }
}
