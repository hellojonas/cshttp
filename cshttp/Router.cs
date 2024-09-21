namespace cshttp;

using System;
using System.Collections.Generic;

public class RouterNode
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

    public RouterNode? lookUp(String path)
    {
        RouterNode cur = root;

        if (path == "/")
        {
            if (cur.children == null)
            {
                return null;
            }

            return cur.children[path];
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

        return cur;
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
