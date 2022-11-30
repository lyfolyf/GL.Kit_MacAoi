using System;

namespace Kit.ORM.FreeSqlHelper
{
    public static class FreeSqlExtension
    {
        public static string GetTableName<T>(this IFreeSql freeSql) where T : class
        {
            return freeSql.CodeFirst.GetTableByEntity(typeof(T)).DbName;
        }

        /// <summary>
        /// 建表
        /// </summary>
        public static void CreateTable<T>(this IFreeSql freeSql) where T : class
        {
            freeSql.CodeFirst.SyncStructure<T>();
        }

        public static void CreateTables(this IFreeSql freeSql, params Type[] entityTypes)
        {
            freeSql.CodeFirst.SyncStructure(entityTypes);
        }
    }
}
