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
    public String? path;
    public String? version;
    public Stream? body;
    public Dictionary<String, String> headers = new Dictionary<String, String>();
}

public class Parser
{
    static byte newLine = 10;
    static byte cReturn = 13;

    public static Request parse(Stream stream)
    {

        byte[] chunk = new byte[1024];
        int offset = 0;

        while (true)
        {
            stream.Read(chunk, offset, 1);
            if (chunk[offset] == cReturn)
            {
                stream.Read(chunk, ++offset, 1);
                if (chunk[offset] == newLine)
                {
                    break;
                }
                throw new Exception("Unexpected <CR> not ollowed by <LF> on first line.");
            }

            offset++;
        }

        String[] firstLine = Encoding.UTF8.GetString(chunk, 0, offset).Split(" ");

        if (firstLine.Length != 3)
        {
            throw new Exception("First line must contain the <METHOD> <RESOURCE> <HTTP_VERSION>.");
        }

        Dictionary<String, String> headers = new Dictionary<String, String>();

        offset = 0;
        while (true)
        {
            stream.Read(chunk, offset, 1);
            if (chunk[offset] != cReturn)
            {
                offset++;
                continue;
            }

            if (!consumeNewLine(stream, chunk, offset + 1))
            {
                throw new Exception("Unexpected <CR> not ollowed by <LF> on headers.");
            }


            String[] header = Encoding.UTF8.GetString(chunk, 0, offset).Split(":");

            if (header.Length != 2)
            {
                throw new Exception("Invalid header '" + header[0].Trim() + "'.");
            }

            headers[header[0].Trim()] = header[1].Trim();

            offset = 0;
            stream.Read(chunk, offset, 1);

            if (chunk[offset] != cReturn)
            {
                offset++;
                continue;
            }

            if (!consumeNewLine(stream, chunk, offset++))
            {
                throw new Exception("Unexpected <CR> not ollowed by <LF> on headers.");
            }

            break;
        }

        return new Request()
        {
            method = firstLine[0].Trim(),
            path = firstLine[1].Trim(),
            version = firstLine[2].Trim(),
            headers = headers,
        };
    }

    static bool consumeNewLine(Stream stream, byte[] chunk, int offset)
    {
        stream.Read(chunk, offset, 1);

        if (chunk[offset] == newLine)
        {
            return true;
        }

        return false;
    }
}
