using System;
using System.Text;
using cshttp;
using cshttp.constants;

Server server = new Server("127.0.0.1", 8080);
Router router = new Router();

router.route("POST", "/resource", (req, res) =>
{
    if (!req.headers.ContainsKey(HttpHeader.ContentLength))
    {
        res.Status(HttpStatus.LENGTH_REQUIRED).Build();
        return;
    }

    int len = int.Parse(req.headers[HttpHeader.ContentLength]);
    byte[] body = new byte[len];
    req.body!.Read(body, 0, len);

    Console.WriteLine($"'{Encoding.UTF8.GetString(body)}'");

    res
    .Status(201)
    .Build();
});

router.route("GET", "/resource", (req, res) =>
{
    String message = "Retrieved Saved Resource!";

    res
    .Header("Content-Type", "text/plain")
    .Header("Content-Length", message.Length.ToString())
    .Write(Encoding.UTF8.GetBytes(message));
});

server.mount(router);
Console.WriteLine("Listening on port: " + 8080);
server.start();
