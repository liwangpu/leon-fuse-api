﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    #region Order 订单
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : EntityBase, IListable, IDTOTransfer<OrderDTO>
    {
        public string Icon { get; set; }
        public string WorkFlowItemId { get; set; }
        public string SubOrderIds { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<OrderFlowLog> OrderFlowLogs { get; set; }

        [NotMapped]
        public string Url { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public OrderDTO ToDTO()
        {
            var dto = new OrderDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Url = Url;
            dto.CategoryName = CategoryName;
            dto.WorkFlowItemId = WorkFlowItemId;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            if (OrderDetails != null)
                dto.OrderDetails = OrderDetails.Select(x => x.ToDTO()).ToList();
            if (OrderFlowLogs != null)
                dto.OrderFlowLogs = OrderFlowLogs;
            return dto;
        }
    }
    #endregion

    #region OrderDTO 订单DTO
    /// <summary>
    /// 订单DTO
    /// </summary>
    public class OrderDTO : EntityBase, IListable
    {
        public string IconAssetId { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string WorkFlowItemId { get; set; }
        public FileAsset IconFileAsset { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public List<OrderFlowLog> OrderFlowLogs { get; set; }
    }
    #endregion

}
