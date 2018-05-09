using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class OrderContent
    {
        public List<OrderContentItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
    }
}
