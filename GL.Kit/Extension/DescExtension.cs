using System;
using System.ComponentModel;
using System.Reflection;

namespace GL.Kit.Extension
{
    public static class DescExtension
    {
        /// <summary>
        /// 获取枚举项的 Description 特性描述
        /// </summary>
        /// <returns>
        /// 返回枚举项的 Description 特性，若无 Description 特性，则返回其 string 值
        /// </returns>
        public static string ToDescription(this Enum @enum)
        {
            Type type = @enum.GetType();

            string name = @enum.ToString();

            FieldInfo field = type.GetField(name);

            if (field == null) return string.Empty;

            object[] descriptionAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptionAttributes.Length > 0)
                return ((DescriptionAttribute)descriptionAttributes[0]).Description;
            else
                return name;
        }

        /// <summary>
        /// 获取类属性的 Description 特性描述
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性名称</param>
        public static string GetPropertyDescription(this Type type, string propertyName)
        {
            PropertyInfo property = type.GetProperty(propertyName);

            if (property == null) return string.Empty;

            object[] descriptionAttributes = property.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptionAttributes.Length > 0)
                return ((DescriptionAttribute)descriptionAttributes[0]).Description;
            else
                return propertyName;
        }
    }
}
