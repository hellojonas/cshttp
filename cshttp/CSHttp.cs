namespace cshttp;

using System;
using System.Net;
using System.Net.Sockets;

public delegate void RequestHandler(Request req, Response res);

public class Server
{
    String host;
    Int32 port;
    Router? router;

    public Server(String host, Int32 port)
    {
        this.host = host;
        this.port = port;
    }

    public void mount(Router router)
    {
        this.router = router;
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

        Request req = RequestParser.parse(stream);
        Response res = new Response(stream);
    }
}

static class Methods
{
    public const String GET = "GET";
    public const String POST = "POST";
    public const String DELETE = "DELETE";
    public const String OPTION = "OPTION";
    public const String CONNECT = "CONNECT";
    public const String PUT = "PUT";
    public const String PATCH = "PATCH";
}

static class Headers
{
    public const String Host = "Host";
    public const String ContentType = "Content-Type";
    public const String ContentLength = "Content-Length";
}
