using System;

namespace ApiModel
{
    #region EntityBase 标准的数据实体(供继承)
    /// <summary>
    /// 标准的数据实体(供继承)
    /// </summary>
    public class EntityBase : DataBase, ICloneable, IEntity
    {
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public int ActiveFlag { get; set; }

        public EntityBase()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                CreatedTime = DateTime.Now;
                ModifiedTime = DateTime.Now;
            }
            else
            {
                ModifiedTime = DateTime.Now;
            }
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
    #endregion
}
