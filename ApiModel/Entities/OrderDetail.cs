using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class OrderDetail : EntityBase, IListable, IDTOTransfer<OrderDetailDTO>
    {
        public string ProductSpecId { get; set; }
        public int Num { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }

        public int OrderDetailStateId { get; set; }
        public Order Order { get; set; }

        [NotMapped]
        public string Icon { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        [NotMapped]
        public ProductSpec ProductSpec { get; set; }


        public OrderDetailDTO ToDTO()
        {
            var dto = new OrderDetailDTO();
            dto.Id = Id;
            dto.ProductSpecId = ProductSpecId;
            dto.Num = Num;
            dto.UnitPrice = UnitPrice;
            dto.TotalPrice = TotalPrice;
            dto.OrderDetailStateId = OrderDetailStateId;
            dto.Remark = Remark;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            if (ProductSpec != null)
            {
                dto.ProductSpecName = ProductSpec.Name;
                if (ProductSpec.Product != null)
                    dto.ProductName = ProductSpec.Product.Name;
                if (ProductSpec.IconFileAsset != null)
                    dto.Icon = ProductSpec.IconFileAsset.Url;
            }

            return dto;
        }
    }

    public class OrderDetailDTO : EntityBase
    {
        public string Icon { get; set; }
        public string ProductSpecId { get; set; }
        public int Num { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
        public int OrderDetailStateId { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecName { get; set; }
    }


}
