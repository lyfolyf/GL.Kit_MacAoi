using FreeSql.DataAnnotations;

namespace GL.FreeSqlKit
{
    public class IdNameState
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        /// <summary>
        /// 状态
        /// 0：已删除；1：正常
        /// </summary>
        [Column(CanUpdate = false)]
        public virtual int State { get; set; }
    }
}
