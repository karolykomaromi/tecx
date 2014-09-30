﻿namespace Hydra
{
    public static class Constants
    {
        public const string ContainerKey = "unity";

        public static class ContentTypes
        {
            public const string Json = "application/json";
        }

        public static class HttpStatusCodes
        {
            public static class Informational1xx
            {
                public const int Continue = 100;
                public const int SwitchingProtocols = 101;
            }

            public static class Successful2xx
            {
                public const int Ok = 200;
                public const int Created = 201;
                public const int Accepted = 202;
                public const int NonAuthoritativeInformation = 203;
                public const int NoContent = 204;
                public const int ResetContent = 205;
                public const int PartialContent = 206;
            }

            public static class Redirection3xx
            {
                public const int MultipleChoices = 300;
                public const int MovedPermanently = 301;
                public const int Found = 302;
                public const int SeeOther = 303;
                public const int NotModified = 304;
                public const int UseProxy = 305;
                public const int TemporaryRedirect = 307;
            }

            public static class ClientError4xx
            {
                public const int BadRequest = 400;
                public const int Unauthorized = 401;
                public const int PaymentRequired = 402;
                public const int Forbidden = 403;
                public const int NotFound = 404;
                public const int MethodNotAllowed = 405;
                public const int NotAcceptable = 406;
                public const int ProxyAuthenticationRequired = 407;
                public const int RequestTimeout = 408;
                public const int Conflict = 409;
                public const int Gone = 410;
                public const int LengthRequired = 411;
                public const int PreconditionFailed = 412;
                public const int RequestEntityTooLarge = 413;
                public const int RequestUriTooLong = 414;
                public const int UnsupportedMediaType = 415;
                public const int RequestedRangeNotSatisfiable = 416;
                public const int ExpectationFailed = 417;
            }

            public static class ServerError5xx
            {
                public const int InternalServerError = 500;
                public const int NotImplemented = 501;
                public const int BadGateway = 502;
                public const int ServiceUnavailable = 503;
                public const int GatewayTimeout = 504;
                public const int HttpVersionNotSupported = 505;
            }
        }
    }
}