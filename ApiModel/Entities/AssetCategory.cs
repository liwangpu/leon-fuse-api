using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class AssetCategory : EntityBase, IListable
    {
        public string Icon { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
        public int DisplayIndex { get; set; }
        public string OrganizationId { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }


        public AssetCategoryDTO ToDTO()
        {
            AssetCategoryDTO dto = new AssetCategoryDTO();
            dto.Id = Id;
            dto.ParentId = ParentId;
            dto.Name = Name;
            dto.Value = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.Type = Type;
            dto.DisplayIndex = DisplayIndex;
            dto.OrganizationId = OrganizationId;
            dto.Children = new List<AssetCategoryDTO>();
            return dto;
        }
    }

    public class AssetCategoryDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
        public string OrganizationId { get; set; }
        /// <summary>
        /// 分类的类型，比如产品product, 材质material
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 在父级分类中的显示顺序，数值为0 - (childrencount - 1)
        /// </summary>
        public int DisplayIndex { get; set; }
        public List<AssetCategoryDTO> Children { get; set; }
    }

    public class AssetCategoryPack
    {
        public List<AssetCategoryDTO> Categories { get; set; }
    }
}
