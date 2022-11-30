using System.ComponentModel;

namespace System
{
    public static class ConvertExtension
    {
        public static object ChanageType(this object value, Type convertsionType)
        {
            if (convertsionType.IsGenericType && convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                convertsionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, convertsionType);
        }
    }
}
