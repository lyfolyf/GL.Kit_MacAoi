using FreeSql;
using FreeSql.Aop;
using GL.Kit.Log;
using System.Linq;

namespace GL.FreeSqlKit
{
    public class FSql
    {
        readonly ILogAdapter _log;

        public IFreeSql FreeSql { get; }

        internal FSql(DataType dbType, string connectionString, bool autoSyncStructure, ILogAdapter log)
        {
            FreeSql = new FreeSqlBuilder()
                .UseConnectionString(dbType, connectionString)
                .UseAutoSyncStructure(autoSyncStructure)
                .Build();

            if (log != null)
            {
                _log = log;

                FreeSql.Aop.CurdBefore += Aop_CurdBefore;
                FreeSql.Aop.CurdAfter += Aop_CurdAfter;
                FreeSql.Aop.SyncStructureAfter += Aop_SyncStructureAfter;
            }
        }

        private void Aop_CurdBefore(object sender, CurdBeforeEventArgs e)
        {
            if (e.DbParms.Length == 0)
            {
                _log.Debug($"SQL 语句\r\n{e.Sql}\r\n");
            }
            else
            {
                _log.Debug($"SQL 语句\r\n{e.Sql}\r\n参数：\r\n - {string.Join("\r\n - ", e.DbParms.Where(pm => pm != null).Select(pm => $"{pm.ParameterName}: {pm.Value}"))}\r\n");
            }
        }

        private void Aop_CurdAfter(object sender, CurdAfterEventArgs e)
        {
            if (e.Exception != null)
            {
                _log.Error("SQL 语句执行出错。", e.Exception);
            }
        }

        private void Aop_SyncStructureAfter(object sender, SyncStructureAfterEventArgs e)
        {
            if (e.Sql != null)
            {
                _log.Debug($"建表语句\r\n{e.Sql}\r\n");
            }

            if (e.Exception != null)
            {
                _log.Error("建表语句执行出错。", e.Exception);
            }
        }
    }
}
