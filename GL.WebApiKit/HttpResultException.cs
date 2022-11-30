using System;
using System.Net;

namespace GL.WebApiKit
{
    public class HttpResultException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public HttpResultException(HttpStatusCode statusCode)
        {
            HttpStatusCode = statusCode;
        }

        public HttpResultException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            HttpStatusCode = statusCode;
        }

        public HttpResultException(HttpStatusCode statusCode, string message, Exception inner)
            : base(message, inner)
        {
            HttpStatusCode = statusCode;
        }
    }
}