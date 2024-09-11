namespace cshttp;

using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


public class Server
{

    String host;
    Int32 port;

    public Server(String host, Int32 port)
    {
        this.host = host;
        this.port = port;
    }

    public void start()
    {
        IPAddress addr = IPAddress.Parse(host);
        TcpListener server = new TcpListener(addr, port);

        server.Start();

        while (true)
        {
            using TcpClient client = server.AcceptTcpClient();
        }
    }

    void handleRequest(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        Request request = Parser.parse(stream);
    }
}
