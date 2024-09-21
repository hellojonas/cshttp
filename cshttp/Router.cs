namespace cshttp;

using System;
using System.Collections.Generic;

public class Router
{
    RouterNode root;

    public Router()
    {
        this.root = new RouterNode();
    }

    public RequestHandler lookUp(String method, String path)
    {
        if (path == "/")
        {
            if (root.children.ContainsKey("/"))
            {
                return root.children["/"].handler;
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

        return cur.handler;
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

class RouterNode
{
    public bool isEnd;
    public String? method;
    public RequestHandler? handler;
    public Dictionary<String, RouterNode> children = new Dictionary<string, RouterNode>();
}
