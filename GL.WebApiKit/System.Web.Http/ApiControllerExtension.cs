using System.Web.Http.Results;

namespace System.Web.Http
{
    public static class ApiControllerExtension
    {
        public static JwtOkResult JwtOk(this ApiController controller, object payload)
        {
            return new JwtOkResult(payload, controller);
        }

        public static JwtOkResult<T> JwtOk<T>(this ApiController controller, object payload, T content)
        {
            return new JwtOkResult<T>(payload, content, controller);
        }
    }
}
