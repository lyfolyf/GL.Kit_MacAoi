using Swashbuckle.Swagger;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GL.WebApiKit.Swagger
{
    /// <summary>
    /// swagger 显示控制器的描述
    /// </summary>
    public class SwaggerControllerDescProvider : ISwaggerProvider
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private static ConcurrentDictionary<string, SwaggerDocument> _cache = new ConcurrentDictionary<string, SwaggerDocument>();
        private readonly string _xmlPath;

        const string Controller = "Controller";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerProvider"></param>
        /// <param name="xmlpath">xml文档路径</param>
        public SwaggerControllerDescProvider(ISwaggerProvider swaggerProvider, string xmlpath)
        {
            _swaggerProvider = swaggerProvider;
            _xmlPath = xmlpath;
        }

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var cacheKey = $"{rootUrl}_{apiVersion}";
            //只读取一次
            if (!_cache.TryGetValue(cacheKey, out SwaggerDocument srcDoc))
            {
                srcDoc = _swaggerProvider.GetSwagger(rootUrl, apiVersion);

                srcDoc.vendorExtensions = new Dictionary<string, object>
                {
                    { "ControllerDesc", GetControllerDesc() }
                };
                _cache.TryAdd(cacheKey, srcDoc);
            }
            return srcDoc;
        }

        /// <summary>
        /// 从API文档中读取控制器描述
        /// </summary>
        /// <returns>所有控制器描述</returns>
        public ConcurrentDictionary<string, string> GetControllerDesc()
        {
            ConcurrentDictionary<string, string> controllerDescDict = new ConcurrentDictionary<string, string>();
            if (File.Exists(_xmlPath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(_xmlPath);

                string[] arrPath;
                int cCount = Controller.Length;
                foreach (XmlNode node in xmldoc.SelectNodes("//member"))
                {
                    string type = node.Attributes["name"].Value;
                    if (type.StartsWith("T:"))
                    {
                        arrPath = type.Split('.');
                        string controllerName = arrPath[arrPath.Length - 1];
                        if (controllerName.EndsWith(Controller))  //控制器
                        {
                            //获取控制器注释
                            XmlNode summaryNode = node.SelectSingleNode("summary");
                            string key = controllerName.Remove(controllerName.Length - cCount, cCount);
                            if (summaryNode != null && !string.IsNullOrEmpty(summaryNode.InnerText) && !controllerDescDict.ContainsKey(key))
                            {
                                controllerDescDict.TryAdd(key, summaryNode.InnerText.Trim());
                            }
                        }
                    }
                }
            }
            return controllerDescDict;
        }
    }
}