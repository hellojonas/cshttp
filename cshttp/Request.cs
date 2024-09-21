namespace cshttp;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class Request
{
    public String? method;
    public String? target;
    public String? version;
    public Stream? body;
    public Dictionary<String, String> headers = new Dictionary<String, String>();
}

public class MessageSyntaxException : Exception
{
    String near { get; }
    public MessageSyntaxException(String message) : base(message)
    {
        this.near = "";
    }
    public MessageSyntaxException(String message, String near) : base(message)
    {
        this.near = near;
    }
}

public class RequestParser
{
    static byte lFeed = 10;
    static byte cReturn = 13;
    static byte colon = 58;
    static byte space = 32;

    public static Request parse(Stream stream)
    {
        Request request = new Request();

        byte[] chunk = new byte[1024];
        byte read;
        int offset = 0;

        while (true)
        {
            read = readByte(stream, chunk, offset++);
            if (read == lFeed || read == cReturn)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("unexpected new line", near);
            }

            if (offset - 1 == 0 && chunk[0] == space)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("Dupliated space character.", near);
            }

            if (read != space)
            {
                continue;
            }

            request.method = Encoding.UTF8.GetString(chunk, 0, offset - 1);
            offset = 0;
            break;
        }

        while (true)
        {
            read = readByte(stream, chunk, offset++);
            if (read == lFeed || read == cReturn)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("unexpected new line", near);
            }

            if (read != space)
            {
                continue;
            }
            if (offset - 1 == 0)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("Dupliated space character.", near);
            }

            request.target = Encoding.UTF8.GetString(chunk, 0, offset - 1);
            offset = 0;
            break;
        }

        while (true)
        {
            read = readByte(stream, chunk, offset++);

            if (read == space || read == lFeed)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("Dupliated space character.", near);
            }

            if (read == cReturn)
            {
                read = readByte(stream, chunk, offset++);

                if (read != lFeed)
                {
                    String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                    throw new MessageSyntaxException("Unexpected CR not followed by LF.", near);
                }

                request.version = Encoding.UTF8.GetString(chunk, 0, offset - 2);
                offset = 0;
                break;
            }

        }

        String headerName;
        while (true)
        {
            read = readByte(stream, chunk, offset++);

            if (read == cReturn)
            {
                read = readByte(stream, chunk, offset++);
                if (read != lFeed)
                {
                    String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                    throw new MessageSyntaxException("Unexpected CR not followed by LF.", near);
                }
                break;
            }

            if (read == space || read == lFeed)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("Invalid request header", near);
            }

            if (read != colon)
            {
                continue;
            }

            read = readByte(stream, chunk, offset++);
            if (read != space)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("Missing space after header name.", near);
            }

            headerName = Encoding.UTF8.GetString(chunk, 0, offset - 2);
            offset = 0;

            read = readByte(stream, chunk, offset++);
            if (read == space)
            {
                String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                throw new MessageSyntaxException("Dupliated space character.", near);
            }

            while (true)
            {
                read = readByte(stream, chunk, offset++);

                if (read == lFeed)
                {
                    String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                    throw new MessageSyntaxException("Unexpected new line.", near);
                }

                if (read != cReturn)
                {
                    continue;
                }

                read = readByte(stream, chunk, offset++);

                if (read != lFeed)
                {
                    String near = offset > 0 ? Encoding.UTF8.GetString(chunk, 0, offset - 1) : "";
                    throw new MessageSyntaxException("Unexpected CR not followed by LF.", near);
                }

                request.headers[headerName] = Encoding.UTF8.GetString(chunk, 0, offset - 2);
                offset = 0;
                break;
            }
        }

        request.body = stream;
        return request;
    }

    static byte readByte(Stream stream, byte[] chunk, int offset)
    {
        stream.Read(chunk, offset, 1);
        return chunk[offset];
    }
}
