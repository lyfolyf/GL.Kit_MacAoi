using System.Collections.Generic;

namespace GL.FreeSqlKit
{
    public interface IIdNameStateRepository<T> where T : IdNameState
    {
        T Add(T entity);

        int Delete(int id);

        /// <summary>
        /// 数据是否存在，由主键验证
        /// <para>用于外键插入时验证</para>
        /// </summary>
        bool Exists(int id);

        /// <summary>
        /// 名称是否已存在
        /// <para>用于插入时验证，不能插入相同 name 值</para>
        /// </summary>
        bool Exists(string name);

        /// <summary>
        /// 名称是否已存在
        /// <para>用于更新时验证，name 可以跟原值相同但不可以其他 name 值相同</para>
        /// </summary>
        bool Exists(int id, string name);

        T GetByID(int id);

        List<T> GetList();

        int Update(T entity);
    }

    public class IdNameStateRepository<T> : IIdNameStateRepository<T> where T : IdNameState
    {
        readonly IFreeSql fsql;

        public IdNameStateRepository(IFreeSql freeSql)
        {
            fsql = freeSql;
        }

        public T Add(T entity)
        {
            entity.ID = (int)fsql
                .Insert(entity)
                .ExecuteIdentity();

            return entity;
        }

        public int Delete(int id)
        {
            return fsql
                .Update<T>(id)
                .Set(a => a.State, StateConsts.Deleted)
                .ExecuteAffrows();
        }

        public bool Exists(string name)
        {
            return fsql
                .Select<T>()
                .Where(a => a.Name == name)
                .Where(a => a.State == StateConsts.Normal)
                .Any();
        }

        public bool Exists(int id, string name)
        {
            return fsql
               .Select<T>()
               .Where(a => a.Name == name)
               .Where(a => a.ID != id)
               .Where(a => a.State == StateConsts.Normal)
               .Any();
        }

        public bool Exists(int id)
        {
            return fsql
                .Select<T>()
                .Where(a => a.ID == id)
                .Where(a => a.State == StateConsts.Normal)
                .Any();
        }

        public T GetByID(int id)
        {
            return fsql
                .Select<T>()
                .Where(a => a.ID == id)
                .Where(a => a.State == StateConsts.Normal)
                .ToOne();
        }

        public List<T> GetList()
        {
            return fsql
                .Select<T>()
                .Where(a => a.State == StateConsts.Normal)
                .ToList();
        }

        public int Update(T entity)
        {
            return fsql
                .Update<T>()
                .SetSource(entity)
                .ExecuteAffrows();
        }
    }
}
