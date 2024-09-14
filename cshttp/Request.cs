namespace cshttp;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

static class Method
{
    public const String GET = "GET";
    public const String POST = "POST";
    public const String DELETE = "DELETE";
    public const String OPTION = "OPTION";
    public const String CONNECT = "CONNECT";
    public const String PUT = "PUT";
    public const String PATCH = "PATCH";
}

public class Request
{
    public String? method;
    public String? target;
    public String? version;
    public Stream? body;
    public Dictionary<String, String> headers = new Dictionary<String, String>();
}

public class Parser
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
                throw new Exception("Invalid request-line method. unexpected new line");
            }

            if (offset - 1 == 0 && chunk[0] == space)
            {
                throw new Exception("Invalid request-line method. extra space not allowed");
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
                throw new Exception("Invalid request-line request-target. unexpected new line");
            }

            if (read != space)
            {
                continue;
            }
            if (offset - 1 == 0)
            {
                throw new Exception("Invalid request-line, extra spacese not allowed");
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
                throw new Exception("Invalid request-line, extra spacese not allowed");
            }

            if (read == cReturn)
            {
                read = readByte(stream, chunk, offset++);

                if (read != lFeed)
                {
                    throw new Exception("Invalid request-line termination. CR not followed by LF.");
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
                    throw new Exception("Unexpected CR not followed by LF.");
                }
                break;
            }

            if (read == space || read == lFeed)
            {
                throw new Exception("Invalid request header");
            }

            if (read != colon)
            {
                continue;
            }

            read = readByte(stream, chunk, offset++);
            if (read != space)
            {
                throw new Exception("Invalid request header. missing space after header name");
            }

            headerName = Encoding.UTF8.GetString(chunk, 0, offset - 2);
            offset = 0;

            while (true)
            {
                read = readByte(stream, chunk, offset++);

                if (read == lFeed || read == space)
                {
                    throw new Exception("Invalid request header. unexpected space character.");
                }

                if (read != cReturn)
                {
                    continue;
                }

                read = readByte(stream, chunk, offset++);

                if (read != lFeed)
                {
                    throw new Exception("Invalid request header. unexpected CR not followed by LF.");
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
