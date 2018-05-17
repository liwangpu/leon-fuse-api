﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Layout : EntityBase, IAsset, IDTOTransfer<LayoutDTO>
    {
        public string CategoryId { get; set; }
        public string Icon { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        /// <summary>
        /// 户型数据,内容为类LayoutData的Json字符串。
        /// </summary>
        public string Data { get; set; }

        public LayoutDTO ToDTO()
        {
            var dto = new LayoutDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.CategoryId = CategoryId;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Modifier = Modifier;
            dto.Data = Data;
            dto.CategoryId = CategoryId;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            return dto;
        }
    }

    public class LayoutDTO : EntityBase
    {
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string CategoryId { get; set; }
        public string Data { get; set; }
    }
}
