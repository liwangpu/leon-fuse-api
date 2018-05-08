using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Order : EntityBase, IListable
    {
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
        [JsonIgnore]
        public Account Account { get; set; }
    }
}
