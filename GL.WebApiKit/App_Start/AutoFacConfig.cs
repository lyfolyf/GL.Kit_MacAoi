using Autofac;
using Autofac.Integration.WebApi;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace GL.WebApiKit
{
    public class AutoFacConfig
    {
        Assembly[] _assemblies;

        public AutoFacConfig(Assembly[] assemblies)
        {
            _assemblies = assemblies;

            // 以下方法可以获取程序使用的所有 DLL，包括 .net framework 框架中的 DLL，太多了，故而没用此方法。
            //System.AppDomain.CurrentDomain.GetAssemblies();
        }

        public void Register(HttpConfiguration config)
        {
            ContainerBuilder builder = new ContainerBuilder();

            //实现服务接口和服务实现的依赖
            //根据名称约定（服务层的接口和实现均以Services结尾）
            builder.RegisterAssemblyTypes(_assemblies)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            //实现数据访问接口和数据访问实现的依赖
            //根据名称约定（数据访问层的接口和实现均以Repository结尾）
            builder.RegisterAssemblyTypes(_assemblies)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            //注册所有的 ApiControllers
            builder.RegisterApiControllers(_assemblies);

            builder.RegisterModule(new AutoMapperModule(_assemblies));

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}