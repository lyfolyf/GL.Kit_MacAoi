using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace GL.WebApiKit.Filter
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode statusCode;
            string message;

            switch (actionExecutedContext.Exception)
            {
                case HttpResultException e:
                    statusCode = e.HttpStatusCode;
                    message = e.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = actionExecutedContext.Exception.Message;
                    break;
            };

            actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(statusCode, message);

            base.OnException(actionExecutedContext);
        }
    }
}