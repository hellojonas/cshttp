using cshttp;
using System.IO;
using System.Text;

namespace cshttp.Tests;

public class ParserTest
{
    [Fact]
    public void ShouldParseMessage()
    {
        String method = "POST";
        String path = "/api/endpoint";
        String version = "HTTP/1.1";
        String host = "example.com";
        String contentType = "application/json";
        String body = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
        int len = body.Length;

        String message =
            method + " " + path + " " + version + "\r\n" +
            "Host: " + host + "\r\n" +
            "Content-Type: " + contentType + "\r\n" +
            "Content-Length: +" + len + "\r\n\r\n" +
            body;

        Request request = Parser.parse(new MemoryStream(Encoding.UTF8.GetBytes(message)));
        int contentLength = int.Parse(request.headers["Conetent-Length"]);

        Assert.Equal(method, request.method);
        Assert.Equal(path, request.path);
        Assert.Equal(host, request.host);
        Assert.Equal(contentType, request.headers["Content-Type"]);
        Assert.Equal(host, request.headers["Host"]);
        Assert.Equal(len, contentLength);

        byte[] data = new byte[len];

        request.body.Read(data, 0, data.Length);

        String strData = Encoding.UTF8.GetString(data);

        Assert.Equal(body, strData);
    }
}
