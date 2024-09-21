namespace cshttp;

using System;
using System.Collections.Generic;

class RouterNode
{
    public bool isEnd;
    public Dictionary<String, RequestHandler>? handlers;
    public Dictionary<String, RouterNode>? children;
}

public class Router
{
    RouterNode root;

    public Router()
    {
        this.root = new RouterNode();
    }

    public RequestHandler? lookUp(String method, String path)
    {
        RouterNode cur = root;

        if (path == "/")
        {
            if (cur.children == null || !cur.children.ContainsKey(path))
            {
                return null;
            }

            cur = cur.children[path];
            if (cur.handlers == null || !cur.handlers.ContainsKey(method))
            {
                return null;
            }

            return cur.handlers[method];
        }

        String[] parts = path.Split("/");

        for (int i = 1; i < parts.Length; i++)
        {
            String part = parts[i];
            if (part == "")
            {
                continue;
            }
            if (cur.children == null || !cur.children.ContainsKey(part))
            {
                return null;

            }
            cur = cur.children[part];
        }

        if (!cur.isEnd)
        {
            return null;
        }

        if (cur.handlers == null || !cur.handlers.ContainsKey(method))
        {
            return null;
        }

        return cur.handlers[method];
    }

    public Router route(String method, String path, RequestHandler handler)
    {
        RouterNode cur = root;
        String part;

        if (path == "/")
        {
            part = path;
            if (cur.children == null)
            {
                cur.children = new Dictionary<String, RouterNode>();
                cur.children[part] = new RouterNode();
            }

            if (!cur.children.ContainsKey(part))
            {
                cur.children[part] = new RouterNode();
            }

            cur = cur.children[part];

            if (cur.handlers == null)
            {
                cur.handlers = new Dictionary<String, RequestHandler>();
            }


            cur.handlers[method] = handler;
            cur.isEnd = true;
            return this;
        }

        String[] parts = path.Split("/");

        for (int i = 1; i < parts.Length; i++)
        {
            part = parts[i];
            if (part == "")
            {
                continue;
            }

            if (cur.children == null)
            {
                cur.children = new Dictionary<String, RouterNode>();
                cur.children[part] = new RouterNode();
            }

            if (!cur.children.ContainsKey(part))
            {
                cur.children[part] = new RouterNode();
            }

            cur = cur.children[part];
        }

        if (cur.handlers == null)
        {
            cur.handlers = new Dictionary<String, RequestHandler>();
        }

        cur.handlers[method] = handler;
        cur.isEnd = true;

        return this;
    }
}
