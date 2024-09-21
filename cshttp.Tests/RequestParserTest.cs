using System;
using System.IO;
using System.Text;

namespace cshttp.Tests;

public class RequestParserTest
{
    [Fact]
    public void shouldParseRequestLine()
    {
        String method = "GET";
        String path = "/api/endpoint";
        String version = "HTTP/1.1";

        String message = method + " " + path + " " + version + "\r\n\r\n";

        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
        Request request = RequestParser.parse(stream);

        Assert.Equal(method, request.method);
        Assert.Equal(path, request.target);
        Assert.Equal(version, request.version);
    }

    [Fact]
    public void ShouldParseMultiValueHeader()
    {
        String method = "POST";
        String path = "/api/endpoint";
        String version = "HTTP/1.1";
        String host = "example.com";
        String encodings = "gzip, deflate, br";
        String contentType = "application/json";
        String body = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
        int len = body.Length;

        String message =
            method + " " + path + " " + version + "\r\n"
            + "Host: " + host + "\r\n"
            + "Content-Type: " + contentType + "\r\n"
            + "Accept-Encoding: " + encodings + "\r\n"
            + "Content-Length: +" + len + "\r\n\r\n"
            + body;

        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
        Request request = RequestParser.parse(stream);

        Assert.Equal(encodings, request.headers["Accept-Encoding"]);

        Assert.Equal(method, request.method);
        Assert.Equal(path, request.target);
        Assert.Equal(version, request.version);

        Assert.Equal(contentType, request.headers["Content-Type"]);
        Assert.Equal(host, request.headers["Host"]);

        int contentLength = int.Parse(request.headers["Content-Length"]);
        Assert.Equal(len, contentLength);

        Assert.NotNull(request.body);

        byte[] data = new byte[len];
        request.body.Read(data, 0, data.Length);
        String strData = Encoding.UTF8.GetString(data);

        Assert.Equal(body, strData);
    }

    [Fact]
    public void ShouldParseCompleteMessage()
    {
        String method = "POST";
        String path = "/api/endpoint";
        String version = "HTTP/1.1";
        String host = "example.com";
        String contentType = "application/json";
        String body = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
        int len = body.Length;

        String message =
            method + " " + path + " " + version + "\r\n"
            + "Host: " + host + "\r\n"
            + "Content-Type: " + contentType + "\r\n"
            + "Content-Length: +" + len + "\r\n\r\n"
            + body;

        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
        Request request = RequestParser.parse(stream);

        Assert.Equal(method, request.method);
        Assert.Equal(path, request.target);
        Assert.Equal(version, request.version);

        Assert.Equal(contentType, request.headers["Content-Type"]);
        Assert.Equal(host, request.headers["Host"]);

        int contentLength = int.Parse(request.headers["Content-Length"]);
        Assert.Equal(len, contentLength);

        Assert.NotNull(request.body);

        byte[] data = new byte[len];
        request.body.Read(data, 0, data.Length);
        String strData = Encoding.UTF8.GetString(data);

        Assert.Equal(body, strData);
    }

    [Fact]
    public void shouldParse()
    {
        String method = "POST";
        String path = "/api/endpoint";
        String version = "HTTP/1.1";
        String body = "{\"key1\":\"value1\",\"key2\":\"value2\"}";

        String message =
            method + " " + path + " " + version + "\r\n\r\n"
            + body;

        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
        Request request = RequestParser.parse(stream);

        Assert.Equal(method, request.method);
        Assert.Equal(path, request.target);
        Assert.Equal(version, request.version);


        int len = body.Length;
        byte[] data = new byte[len];

        Assert.NotNull(request.body);
        request.body.Read(data, 0, len);

        String strData = Encoding.UTF8.GetString(data);

        Assert.Equal(body, strData);
    }

    [Fact]
    public void shouldFailInvalidSpacing()
    {
        String method = "POST";
        String path = "/api/endpoint";
        String version = "HTTP/1.1";
        String host = "example.com";
        String contentType = "application/json";
        String body = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
        int len = body.Length;

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                method + "  " + path + " " + version + "\r\n"
                + "Host: " + host + "\r\n"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                method + " " + path + "  " + version + "\r\n"
                + "Host: " + host + "\r\n"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                method + " " + path + " " + version + "\n"
                + "Host: " + host + "\r\n"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                method + " " + path + " " + version + "\r\n"
                + "Host: " + host + "\r"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                method + " " + path + " " + version + "\r\n"
                + "Host: " + host + "\r\n"
                + "Content-Type: " + contentType + "\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                " " + method + " " + path + " " + version + "\r\n"
                + " Host: " + host + "\r\n"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });

        Assert.Throws<MessageSyntaxException>(() =>
        {
            String message =
                method + " " + path + " " + version + "\r\n"
                + " Host: " + host + "\r\n"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: +" + len + "\r\n\r\n"
                + body;
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            Request request = RequestParser.parse(stream);
        });
    }
}
