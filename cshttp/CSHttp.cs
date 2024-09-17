namespace cshttp;

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


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
            handleRequest(client);
        }
    }

    void handleRequest(TcpClient client)
    {
        NetworkStream stream = client.GetStream();

        Request req = Parser.parse(stream);
        Response res = new Response(stream);
    }

    void route(String method, String path, RequestHandler handler)
    {
    }
}

public delegate void RequestHandler(Request req, Response res);

public class HandlerNode
{
    public RequestHandler Handler;
    public Dictionary<(String, String), HandlerNode> part;
}
