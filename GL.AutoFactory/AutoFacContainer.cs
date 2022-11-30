using Autofac.Core;
using System;

namespace Autofac
{
    public class AutoFacContainer
    {
        static _AutoFacContainer container = new _AutoFacContainer();

        public static void Register(Action<ContainerBuilder> action)
        {
            container.Register(action);
        }

        public static T Resolve<T>(params Parameter[] parameters)
        {
            return container.Resolve<T>(parameters);
        }

        public static object Resolve(Type type, params Parameter[] parameters)
        {
            return container.Resolve(type, parameters);
        }

        public static void AfterResolve(Action<object> action)
        {
            container.AfterResolve += action;
        }

        class _AutoFacContainer
        {
            IContainer _container;

            public event Action<object> AfterResolve;

            public void Register(Action<ContainerBuilder> action)
            {
                ContainerBuilder builder = new ContainerBuilder();

                action(builder);

                _container = builder.Build();
            }

            public T Resolve<T>(params Parameter[] parameters)
            {
                T t = _container.Resolve<T>(parameters);

                AfterResolve?.Invoke(t);

                return t;
            }

            public object Resolve(Type type, params Parameter[] parameters)
            {
                object obj = _container.Resolve(type, parameters);

                AfterResolve?.Invoke(obj);

                return obj;
            }
        }
    }
}
