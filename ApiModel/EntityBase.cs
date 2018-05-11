using ApiModel.Consts;
using ApiModel.Enums;
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
        /// <summary>
        /// 启用状态 1启用 0禁用
        /// </summary>
        public int ActiveFlag { get; set; }
        /// <summary>
        /// 资源开放类型 具体查看ResourceTypeEnum
        /// </summary>
        public int ResourceType { get; set; }
        public EntityBase()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                CreatedTime = DateTime.Now;
                ModifiedTime = DateTime.Now;
                ActiveFlag = AppConst.I_DataState_Active;
                ResourceType = (int)ResourceTypeEnum.Personal;
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
