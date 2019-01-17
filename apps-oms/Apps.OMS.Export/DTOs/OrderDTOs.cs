using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.OMS.Export.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string CreatorName { get; set; }
        public string ModifierName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string OrganizationId { get; set; }
        public string OrderNo { get; set; }
        public int TotalNum { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        //public List<OrderFlowLog> OrderFlowLogs { get; set; }
    }

    public class OrderDetailDTO
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string ProductSpecId { get; set; }
        public string ProductSpecName { get; set; }
        public string ProductId { get; set; }
        public string ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductUnit { get; set; }
        public string ProductDescription { get; set; }
        public int Num { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
        public string AttachmentIds { get; set; }
    }
}
