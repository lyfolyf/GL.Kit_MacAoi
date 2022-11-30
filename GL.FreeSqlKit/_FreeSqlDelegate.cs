using System;
using System.Linq.Expressions;

namespace Kit.ORM.FreeSqlHelper
{
    public class FreeSqlDelegate
    {
        /// <summary>
        /// 初始化表数据
        /// <para>1、如果表中无数据，则插入指定数据</para>
        /// <para>2、如果表中有数据，且数据量与指定数据量不同，则删除原有数据后插入指定数据</para>
        /// <para>3、如果表中有数据，且数据量与指定数据量相同，则不进行任何操作</para>
        /// </summary>
        /// <remarks>
        /// 这里应用了闭包，所以必须返回方法
        /// </remarks>
        public static Action<IFreeSql> InitTable<T>(T[] data) where T : class
        {
            return delegate (IFreeSql db)
            {
                if (data != null)
                {
                    int count = (int)db.Select<T>().Count();
                    if (count == 0)
                    {
                        db.Insert(data).ExecuteAffrows();
                    }
                    else if (count != data.Length)
                    {
                        db.Delete<T>().Where(a => 1 == 1).ExecuteAffrows();
                        db.Insert(data).ExecuteAffrows();
                    }
                }
            };
        }

        /// <summary>
        /// 初始化表数据
        /// <para>1、如果表中无数据，则插入指定数据</para>
        /// <para>2、如果表中有数据，且数据量与指定数据量不同，则删除原有数据后插入指定数据</para>
        /// <para>3、如果表中有数据，且数据量与指定数据量相同，则不进行任何操作</para>
        /// </summary>
        /// <remarks>
        /// 这里应用了闭包，所以必须返回方法
        /// </remarks>
        public static void InitTable<T>(IFreeSql db, T[] data) where T : class
        {
            if (data != null)
            {
                int count = (int)db.Select<T>().Count();
                if (count == 0)
                {
                    db.Insert(data).ExecuteAffrows();
                }
                else if (count != data.Length)
                {
                    db.Delete<T>().Where(a => 1 == 1).ExecuteAffrows();
                    db.Insert(data).ExecuteAffrows();
                }
            }
        }

        /// <summary>
        /// 如果数据不存在则插入
        /// </summary>
        /// <param name="exp">判断数据是否存在的条件</param>
        public static Action<IFreeSql> InsertWhenNotExists<T>(T data, Expression<Func<T, bool>> exp) where T : class
        {
            return delegate (IFreeSql db)
            {
                bool b = db.Select<T>().Where(exp).Any();
                if (b == false)
                {
                    db.Insert(data).ExecuteAffrows();
                }
            };
        }

        /// <summary>
        /// 如果数据不存在则插入
        /// </summary>
        /// <param name="exp">判断数据是否存在的条件</param>
        public static void InsertWhenNotExists<T>(IFreeSql db, T data, Expression<Func<T, bool>> exp) where T : class
        {
            bool b = db.Select<T>().Where(exp).Any();
            if (b == false)
            {
                db.Insert(data).ExecuteAffrows();
            }
        }
    }
}
