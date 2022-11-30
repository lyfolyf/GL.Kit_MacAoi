using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System.Text;

namespace GL.WebApiKit.JWT
{
    public class JwtHelper
    {
        // 加密方式
        static readonly IJwtAlgorithm algorithm = new HMACSHA256Algorithm();

        // 序列化 Json
        static readonly IJsonSerializer jsonSerializer = new JsonNetSerializer();

        // base64 编码器
        static readonly IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();

        // UTC 时间获取
        static readonly IDateTimeProvider dateTimeProvider = new UtcDateTimeProvider();

        // 为了在 JWTAuthorizeAttribute 中取消掉 Key，暂时没想到更好的方法
        public string DefaultKey { get; set; } = "1234567890";

        /// <summary>
        /// 使用默认密钥生成 JWT
        /// </summary>
        public string Encode(object payload)
        {
            return Encode(payload, DefaultKey);
        }

        /// <summary>
        /// 生成 JWT
        /// </summary>
        /// <param name="payload">需要编码的内容，必须可序列化为 json</param>
        /// <param name="key">密钥</param>
        public string Encode(object payload, string key)
        {
            // JWT 编码器
            IJwtEncoder encoder = new JwtEncoder(algorithm, jsonSerializer, urlEncoder);

            // 生成令牌
            return encoder.Encode(payload, Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// 使用默认密钥解码 JWT
        /// </summary>
        public string Decode(string token)
        {
            return Decode(token, DefaultKey);
        }

        /// <summary>
        /// 解码 JWT
        /// </summary>
        /// <param name="token">JWT</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public string Decode(string token, string key)
        {
            IJwtValidator jwtValidator = new JwtValidator(jsonSerializer, dateTimeProvider);

            IJwtDecoder decoder = new JwtDecoder(jsonSerializer, jwtValidator, urlEncoder, algorithm);

            return decoder.Decode(token, key, true);
        }
    }
}
