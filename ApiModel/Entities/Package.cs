using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    #region Order 套餐
    /// <summary>
    /// 套餐
    /// </summary>
    public class Package : EntityBase, IListable, IDTOTransfer<PackageDTO>
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图标Asset Id
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 套餐内容, 内容为OrderContent对象的json字符串
        /// </summary>
        public string Content { get; set; }

        [NotMapped]
        public PackageContent ContentIns { get; set; }

        #region ToDTO
        public PackageDTO ToDTO()
        {
            var dto = new PackageDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Content = Content;
            if (ContentIns != null)
                dto.ContentIns = ContentIns;
            return dto;
        } 
        #endregion
    }
    #endregion

    #region OrderDTO 套餐DTO
    /// <summary>
    /// 套餐DTO
    /// </summary>
    public class PackageDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string State { get; set; }
        public string Content { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public PackageContent ContentIns { get; set; }
    }
    #endregion


    #region OrderContent 套餐内容(仅供序列化使用,非数据库实体)
    /// <summary>
    /// 套餐内容(仅供序列化使用,非数据库实体)
    /// </summary>
    public class PackageContent
    {
        public List<PackageContentItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
    }
    #endregion

    #region OrderContentItem 套餐项(仅供序列化使用,非数据库实体)
    /// <summary>
    /// 套餐项(仅供序列化使用,非数据库实体)
    /// </summary>
    public class PackageContentItem
    {
        public string ProductSpecId { get; set; }
        public int Num { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecName { get; set; }
    }
    #endregion
}
