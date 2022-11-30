using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GL.Kit.Reflection
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// 反射获取程序集中满足筛选条件的 Type
        /// </summary>
        /// <param name="typeFilterFunc">筛选条件函数</param>
        /// <param name="assemblies">要搜索的程序集</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes(Func<Type, bool> typeFilterFunc, params Assembly[] assemblies)
        {
            return assemblies.Select(a => a.GetTypes().Where(t => typeFilterFunc(t))).SelectMany(ts => ts);
        }

        /// <summary>
        /// 反射获取程序集中继承指令接口的类
        /// </summary>
        public static IEnumerable<Type> GetTypes(this Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes().Where(t => interfaceType.IsAssignableFrom(t));
        }
    }
}
