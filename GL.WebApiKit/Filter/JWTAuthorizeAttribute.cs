using GL.WebApiKit.JWT;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GL.WebApiKit.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JWTAuthorizeAttribute : AuthorizeAttribute
    {
        const string Authorization = "Authorization";

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.TryGetValues(Authorization, out var tokenHeader);

            string token = tokenHeader?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(token)) return false;

            try
            {
                string payload = JwtSingle.Instance.JWT.Decode(token);
                actionContext.RequestContext.RouteData.Values.Add(Authorization, payload);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 处理授权失败的请求
        /// </summary>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Token 认证失败。");
        }
    }
}

