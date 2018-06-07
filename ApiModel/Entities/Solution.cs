using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Solution : EntityBase, IAsset, IListable, IDTOTransfer<SolutionDTO>
    {
        public string Icon { get; set; }
        public string LayoutId { get; set; }


        public Layout Layout { get; set; }

        /// <summary>
        /// 内容为 SolutionData 对象的 json字符串
        /// </summary>
        public string Data { get; set; }
        public string CategoryId { get; set; }
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
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            return dto;
        }
    }

    public class SolutionDTO : EntityBase
    {
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string CategoryId { get; set; }
        public string Data { get; set; }
        public string LayoutId { get; set; }
    }
}
