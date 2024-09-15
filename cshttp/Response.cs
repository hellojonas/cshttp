namespace cshttp;

using System;
using System.IO;
using System.Collections.Generic;

public class Response
{
    public int? status;
    public Stream stream { get; }
    public Dictionary<String, String> headers = new Dictionary<String, String>();

    public Response(Stream stream)
    {
        this.stream = stream;
    }
}
