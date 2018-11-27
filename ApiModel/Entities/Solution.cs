﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Solution : EntityBase, IAsset, IListable, IDTOTransfer<SolutionDTO>
    {
        public string Icon { get; set; }
        public string LayoutId { get; set; }
        public bool Snapshot { get; set; }
        public Layout Layout { get; set; }

        /// <summary>
        /// 内容为 SolutionData 对象的 json字符串
        /// </summary>
        public string Data { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public SolutionDTO ToDTO()
        {
            var dto = new SolutionDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.CategoryId = CategoryId;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Data = Data;
            dto.LayoutId = LayoutId;
            dto.CategoryName = CategoryName;
            dto.ResourceType = ResourceType;
            dto.Snapshot = Snapshot;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            return dto;
        }
    }

    public class SolutionDTO : EntityBase, IListable
    {
        public bool Snapshot { get; set; }
        public string IconAssetId { get; set; }
        public string Data { get; set; }
        public string LayoutId { get; set; }
        public string Icon { get; set; }
        public FileAsset IconFileAsset { get; set; }
    }
}
