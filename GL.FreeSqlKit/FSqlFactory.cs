using FreeSql;
using GL.Kit.Log;
using System;
using System.Collections.Generic;

namespace GL.FreeSqlKit
{
    public class FSqlFactory
    {
        static Dictionary<string, FSql> fsqlDict = new Dictionary<string, FSql>();

        public static void Register(string key, DataType dbType, string connectionString, bool autoSyncStructure, ILogAdapter log)
        {
            if (fsqlDict.ContainsKey(key))
                throw new Exception($"数据库连接 {key} 已注册。");

            fsqlDict.Add(key, new FSql(dbType, connectionString, autoSyncStructure, log));
        }

        public static IFreeSql GetFSql(string key)
        {
            if (fsqlDict.ContainsKey(key))
                return fsqlDict[key].FreeSql;

            throw new Exception($"数据库连接 {key} 未注册。");
        }
    }
}
