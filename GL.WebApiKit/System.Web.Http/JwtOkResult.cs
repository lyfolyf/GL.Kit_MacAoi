using GL.WebApiKit.JWT;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Http.Results
{
    public class JwtOkResult : IHttpActionResult
    {
        readonly object m_payload;
        readonly ApiController m_controller;

        public JwtOkResult(object payload, ApiController controller)
        {
            m_payload = payload;
            m_controller = controller;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = m_controller.Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Authorization", JwtSingle.Instance.JWT.Encode(m_payload));

            return Task.FromResult(response);
        }
    }

    public class JwtOkResult<T> : IHttpActionResult
    {
        readonly object m_payload;
        readonly ApiController m_controller;
        readonly T m_content;

        public JwtOkResult(object payload, T content, ApiController controller)
        {
            m_payload = payload;
            m_content = content;
            m_controller = controller;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = m_controller.Request.CreateResponse<T>(HttpStatusCode.OK, m_content);
            response.Headers.Add("Authorization", JwtSingle.Instance.JWT.Encode(m_payload));

            return Task.FromResult(response);
        }
    }
}
