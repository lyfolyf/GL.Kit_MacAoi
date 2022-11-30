using System;

namespace GL.NpoiKit
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DataLabelAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// 导出时使用
        /// </summary>
        public int Order { get; set; }

        public bool NotMapped { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ClassLabelAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
