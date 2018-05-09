using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : EntityBase, IListable, IPermission, IDTOTransfer<OrderDTO>
    {
        public Order()
        {
            StateTime = DateTime.Now;
        }

        public string Description { get; set; }
        public string Icon { get; set; }
        public string AccountId { get; set; }
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
        public Account Account { get; set; }

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
}
