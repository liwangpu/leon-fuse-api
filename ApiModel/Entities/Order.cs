using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    #region Order 订单
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : EntityBase, IListable, IDTOTransfer<OrderDTO>
    {
        public Order()
        {
            StateTime = DateTime.Now;
        }
        /// <summary>
        /// 图标Asset Id
        /// </summary>
        public string Icon { get; set; }
        public string State { get; set; }
        public DateTime StateTime { get; set; }
        /// <summary>
        /// 子订单列表，由多个订单id用分号分隔而成
        /// </summary>
        public string ChildOrders { get; set; }
        /// <summary>
        /// 订单内容, 内容为OrderContent对象的json字符串
        /// </summary>
        public string Content { get; set; }

        public List<OrderStateItem> OrderStates { get; set; }


        [NotMapped]
        public OrderContent ContentIns { get; set; }

        public OrderDTO ToDTO()
        {
            var dto = new OrderDTO();
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
    }
    #endregion

    #region OrderDTO 订单DTO
    /// <summary>
    /// 订单DTO
    /// </summary>
    public class OrderDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string AccountId { get; set; }
        public string State { get; set; }
        public string Content { get; set; }
        public DateTime StateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public OrderContent ContentIns { get; set; }
    } 
    #endregion

    #region OrderContent 订单内容(仅供序列化使用,非数据库实体)
    /// <summary>
    /// 订单内容(仅供序列化使用,非数据库实体)
    /// </summary>
    public class OrderContent
    {
        public List<OrderContentItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
    }
    #endregion

    #region OrderContentItem 订单项(仅供序列化使用,非数据库实体)
    /// <summary>
    /// 订单项(仅供序列化使用,非数据库实体)
    /// </summary>
    public class OrderContentItem
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
