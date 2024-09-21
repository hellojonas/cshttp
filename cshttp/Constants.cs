namespace cshttp.constants;
using System.Collections.Generic;

using System;

public static class HttpMethod
{
    public const String GET = "GET";
    public const String POST = "POST";
    public const String DELETE = "DELETE";
    public const String OPTION = "OPTION";
    public const String CONNECT = "CONNECT";
    public const String PUT = "PUT";
    public const String PATCH = "PATCH";
}

public static class HttpHeader
{
    public const String Host = "Host";
    public const String ContentType = "Content-Type";
    public const String ContentLength = "Content-Length";
}

public static class HttpStatus
{
    // 1xx Informational
    public static int CONTINUE = 100;
    public static int SWITCHING_PROTOCOLS = 101;
    public static int PROCESSING = 102;
    public static int EARLY_HINTS = 103;

    // 2xx Success
    public static int OK = 200;
    public static int CREATED = 201;
    public static int ACCEPTED = 202;
    public static int NON_AUTHORITATIVE_INFORMATION = 203;
    public static int NO_CONTENT = 204;
    public static int RESET_CONTENT = 205;
    public static int PARTIAL_CONTENT = 206;
    public static int MULTI_STATUS = 207;
    public static int ALREADY_REPORTED = 208;
    public static int IM_USED = 226;

    // 3xx Redirection
    public static int MULTIPLE_CHOICES = 300;
    public static int MOVED_PERMANENTLY = 301;
    public static int FOUND = 302;
    public static int SEE_OTHER = 303;
    public static int NOT_MODIFIED = 304;
    public static int TEMPORARY_REDIRECT = 307;
    public static int PERMANENT_REDIRECT = 308;

    // 4xx Client Errors
    public static int BAD_REQUEST = 400;
    public static int UNAUTHORIZED = 401;
    public static int PAYMENT_REQUIRED = 402;
    public static int FORBIDDEN = 403;
    public static int NOT_FOUND = 404;
    public static int METHOD_NOT_ALLOWED = 405;
    public static int NOT_ACCEPTABLE = 406;
    public static int PROXY_AUTHENTICATION_REQUIRED = 407;
    public static int REQUEST_TIMEOUT = 408;
    public static int CONFLICT = 409;
    public static int GONE = 410;
    public static int LENGTH_REQUIRED = 411;
    public static int PRECONDITION_FAILED = 412;
    public static int PAYLOAD_TOO_LARGE = 413;
    public static int URI_TOO_LONG = 414;
    public static int UNSUPPORTED_MEDIA_TYPE = 415;
    public static int RANGE_NOT_SATISFIABLE = 416;
    public static int EXPECTATION_FAILED = 417;
    public static int IM_A_TEAPOT = 418;
    public static int MISDIRECTED_REQUEST = 421;
    public static int UNPROCESSABLE_ENTITY = 422;
    public static int LOCKED = 423;
    public static int FAILED_DEPENDENCY = 424;
    public static int TOO_EARLY = 425;
    public static int UPGRADE_REQUIRED = 426;
    public static int PRECONDITION_REQUIRED = 428;
    public static int TOO_MANY_REQUESTS = 429;
    public static int REQUEST_HEADER_FIELDS_TOO_LARGE = 431;
    public static int UNAVAILABLE_FOR_LEGAL_REASONS = 451;

    // 5xx Server Errors
    public static int INTERNAL_SERVER_ERROR = 500;
    public static int NOT_IMPLEMENTED = 501;
    public static int BAD_GATEWAY = 502;
    public static int SERVICE_UNAVAILABLE = 503;
    public static int GATEWAY_TIMEOUT = 504;
    public static int HTTP_VERSION_NOT_SUPPORTED = 505;
    public static int VARIANT_ALSO_NEGOTIATES = 506;
    public static int INSUFFICIENT_STORAGE = 507;
    public static int LOOP_DETECTED = 508;
    public static int NOT_EXTENDED = 510;
    public static int NETWORK_AUTHENTICATION_REQUIRED = 511;
}


public static class HttpStatusMessages
{
    public static readonly Dictionary<int, string> StatusDescriptions = new Dictionary<int, string>
    {
        // 1xx Informational
        {100, "Continue"},
        {101, "Switching Protocols"},
        {102, "Processing"},
        {103, "Early Hints"},

        // 2xx Success
        {200, "OK"},
        {201, "Created"},
        {202, "Accepted"},
        {203, "Non-Authoritative Information"},
        {204, "No Content"},
        {205, "Reset Content"},
        {206, "Partial Content"},
        {207, "Multi-Status"},
        {208, "Already Reported"},
        {226, "IM Used"},

        // 3xx Redirection
        {300, "Multiple Choices"},
        {301, "Moved Permanently"},
        {302, "Found"},
        {303, "See Other"},
        {304, "Not Modified"},
        {305, "Use Proxy"},
        {306, "Switch Proxy"}, // No longer used
        {307, "Temporary Redirect"},
        {308, "Permanent Redirect"},

        // 4xx Client Errors
        {400, "Bad Request"},
        {401, "Unauthorized"},
        {402, "Payment Required"},
        {403, "Forbidden"},
        {404, "Not Found"},
        {405, "Method Not Allowed"},
        {406, "Not Acceptable"},
        {407, "Proxy Authentication Required"},
        {408, "Request Timeout"},
        {409, "Conflict"},
        {410, "Gone"},
        {411, "Length Required"},
        {412, "Precondition Failed"},
        {413, "Payload Too Large"},
        {414, "URI Too Long"},
        {415, "Unsupported Media Type"},
        {416, "Range Not Satisfiable"},
        {417, "Expectation Failed"},
        {418, "I'm a teapot"},
        {421, "Misdirected Request"},
        {422, "Unprocessable Entity"},
        {423, "Locked"},
        {424, "Failed Dependency"},
        {425, "Too Early"},
        {426, "Upgrade Required"},
        {428, "Precondition Required"},
        {429, "Too Many Requests"},
        {431, "Request Header Fields Too Large"},
        {451, "Unavailable For Legal Reasons"},

        // 5xx Server Errors
        {500, "Internal Server Error"},
        {501, "Not Implemented"},
        {502, "Bad Gateway"},
        {503, "Service Unavailable"},
        {504, "Gateway Timeout"},
        {505, "HTTP Version Not Supported"},
        {506, "Variant Also Negotiates"},
        {507, "Insufficient Storage"},
        {508, "Loop Detected"},
        {510, "Not Extended"},
        {511, "Network Authentication Required"}
    };
}
