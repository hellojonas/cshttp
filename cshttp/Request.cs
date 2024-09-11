namespace cshttp;

using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;

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
    public String? host;
    public String? path;
    public String? method;
    public NetworkStream? body;
    public Dictionary<String, String> headers = new Dictionary<String, String>();
}

public class Parser
{

    public static Request parse(Stream stream)
    {
    }
}
