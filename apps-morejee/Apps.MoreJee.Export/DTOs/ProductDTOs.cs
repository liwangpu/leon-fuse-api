using System;
using System.Collections.Generic;

namespace Apps.MoreJee.Export.DTOs
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string CreatorName { get; set; }
        public string ModifierName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int ActiveFlag { get; set; }
        public string OrganizationId { get; set; }
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductSpecDTO> Specifications { get; set; }
    }


    public class ProductSpecDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string CreatorName { get; set; }
        public string ModifierName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string IconAssetId { get; set; }
        public decimal Price { get; set; }
        public decimal PartnerPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string TPID { get; set; }
        public string ProductId { get; set; }
        public List<StaticMeshDTO> StaticMeshes { get; set; }
        public string Icon { get; set; }
        public string Components { get; set; }
        public string Description { get; set; }
    }
}
