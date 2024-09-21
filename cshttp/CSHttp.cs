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

        Response res = new Response(stream);
        Request? req = null;

        try
        {
            req = RequestParser.parse(stream);
        }
        catch (MessageSyntaxException)
        {
            res.Status(((int)HttpStatusCode.BadRequest)).Build();
            return;
        }

        if (router == null)
        {
            res.Status(((int)HttpStatusCode.NotFound)).Build();
            return;
        }

        RouterNode? node = router.lookUp(req.target);

        if (node == null)
        {
            res.Status(((int)HttpStatusCode.NotFound)).Build();
            return;
        }

        if (node.handlers == null)
        {
            res.Status(((int)HttpStatusCode.InternalServerError)).Build();
            throw new Exception("Router node without handler.");
        }

        if (!node.handlers.ContainsKey(req.method))
        {
            res.Status(((int)HttpStatusCode.MethodNotAllowed)).Build();
            return;
        }

        node.handlers[req.method](req, res);
    }
}
