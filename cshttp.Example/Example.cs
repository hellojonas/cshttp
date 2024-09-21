using System;
using System.Collections.Generic;
using System.Text;
using cshttp;
using cshttp.constants;

String addr = "127.0.0.1";
Int32 port = 8080;

Server server = new Server(addr, port);
Router router = new Router();

List<String> messages = new List<string>();

router.route("POST", "/messages", (req, res) =>
{
    if (!req.headers.ContainsKey(HttpHeader.ContentLength))
    {
        res.Status(HttpStatus.LENGTH_REQUIRED).Build();
        return;
    }

    int len = int.Parse(req.headers[HttpHeader.ContentLength]);
    byte[] body = new byte[len];
    req.body!.Read(body, 0, len);

    messages.Add(Encoding.UTF8.GetString(body));

    res
    .Status(201)
    .Build();
});

router.route("GET", "/messages", (req, res) =>
{
    String msgs = String.Join("\n", messages);

    res
    .Header("Content-Type", "text/plain")
    .Header("Content-Length", msgs.Length.ToString())
    .Write(Encoding.UTF8.GetBytes(msgs));
});

server.mount(router);
Console.WriteLine($"Application listentin on {addr}:{port}");
server.start();
