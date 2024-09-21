namespace cshttp;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Response
{
    int status { set; get; }
    Stream stream;
    Dictionary<String, String> headers;

    public Response(Stream stream)
    {
        this.stream = stream;
        this.headers = new Dictionary<String, String>();
        this.status = 200;
    }

    public Response Status(int status)
    {
        this.status = status;
        return this;
    }

    public Response Header(String key, String value)
    {
        headers[key] = value;
        return this;
    }

    public void Write(byte[] body)
    {
        byte[] header = Encoding.UTF8.GetBytes(createMessageHeader());
        byte[] message = new byte[header.Length + body.Length];

        header.CopyTo(message, 0);
        body.CopyTo(message, header.Length);

        stream.Write(message);
        stream.Flush();
        stream.Close();

    }

    public void Build()
    {
        stream.Write(Encoding.UTF8.GetBytes(createMessageHeader()));
        stream.Flush();
        stream.Close();
    }

    String createMessageHeader()
    {
        StringBuilder builder = new StringBuilder($"HTTP/1.1 {status} OK\r\n");

        foreach (var entry in headers)
        {
            builder.Append($"{entry.Key}: {entry.Value}\r\n");
        }

        builder.Append("\r\n");

        return builder.ToString();
    }
}
