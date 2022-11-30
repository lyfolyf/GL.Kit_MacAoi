using GL.WebApiKit.Filter;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace GL.WebApiKit.Swagger
{
    // 实测加在 Controller 上无效，暂未找到合适方法
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerHttpHeaderAttribute : Attribute
    {
        public string Name { get; }

        public string Description { get; }

        public SwaggerHttpHeaderAttribute(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Description = description;
        }
    }

    public class SwaggerAuthorizationHeaderAttribute : SwaggerHttpHeaderAttribute
    {
        public SwaggerAuthorizationHeaderAttribute()
            : base("Authorization", "令牌")
        {
        }
    }

    public class SwaggerHttpHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            var heads = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerHttpHeaderAttribute>();
            foreach (var headattr in heads)
            {
                operation.parameters.Add(new Parameter { name = headattr.Name, @in = "header", description = headattr.Description, required = false, type = "string" });
            }
        }
    }
}
