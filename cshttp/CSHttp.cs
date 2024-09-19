namespace cshttp;

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


public class Server
{
    String host;
    Int32 port;
    Router router;

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

        Request req = Parser.parse(stream);
        Response res = new Response(stream);
    }
}

public delegate void RequestHandler(Request req, Response res);

public class Router
{
    RouterNode root;

    public Router()
    {
        this.root = new RouterNode();
    }

    public RouterNode lookUp(String method, String path)
    {
        if (path == "/")
        {
            if (root.children.ContainsKey("/"))
            {
                return root.children["/"];
            }
            else
            {
                return null;
            }
        }

        RouterNode cur = root;
        String[] parts = path.Split("/");


        for (int i = 1; i < parts.Length; i++)
        {
            String part = parts[i];
            if (part == "")
            {
                continue;
            }
            if (cur.children.ContainsKey(part))
            {
                cur = cur.children[part];
            }
            else
            {
                return null;
            }
        }

        return cur;
    }

    public void route(String method, String path, RequestHandler handler)
    {
        if (path == "/")
        {
            root.children["/"] = new RouterNode()
            {
                method = method,
                handler = handler,
                isEnd = true,
            };
            return;
        }

        RouterNode cur = root;
        String[] parts = path.Split("/");

        for (int i = 1; i < parts.Length; i++)
        {
            String part = parts[i];
            if (part == "")
            {
                continue;
            }

            if (!cur.children.ContainsKey(part))
            {
                cur.children[part] = new RouterNode();
            }

            cur = cur.children[part];
        }

        cur.method = method;
        cur.handler = handler;
        cur.isEnd = true;
    }
}

public class RouterNode
{
    public bool isEnd;
    public String? method;
    public RequestHandler? handler;
    public Dictionary<String, RouterNode> children = new Dictionary<string, RouterNode>();
}
