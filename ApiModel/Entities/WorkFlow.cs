using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApiModel.Entities
{
    public class WorkFlow : EntityBase, IListable, IDTOTransfer<WorkFlowDTO>
    {
        public string Icon { get; set; }
        /// <summary>
        /// 适用组织类型,逗号分隔
        /// </summary>
        public string ApplyOrgans { get; set; }
        public List<WorkFlowItem> WorkFlowItems { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public WorkFlowDTO ToDTO()
        {
            var dto = new WorkFlowDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.ActiveFlag = ActiveFlag;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.ApplyOrgans = ApplyOrgans;
            return dto;
        }
    }

    public class WorkFlowDTO : EntityBase, IListable
    {
        public string Icon { get; set; }
        public string ApplyOrgans { get; set; }
        public FileAsset IconFileAsset { get; set; }
        public List<WorkFlowItem> WorkFlowItems { get; set; }
    }
}
