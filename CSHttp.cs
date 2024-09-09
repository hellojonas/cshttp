namespace cshttp;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class CSHttp
{
    public void listen(String host, Int32 port)
    {
        IPAddress addr = IPAddress.Parse(host);
        TcpListener server = new TcpListener(addr, port);

        server.Start();

        while (true)
        {
            using TcpClient client = server.AcceptTcpClient();
        }
    }

    public void handleRequest(TcpClient client)
    { // TODO: process client request
    }
}
