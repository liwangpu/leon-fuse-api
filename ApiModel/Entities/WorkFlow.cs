using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApiModel.Entities
{
    public class WorkFlow : EntityBase, IListable, IDTOTransfer<WorkFlowDTO>
    {
        public string Icon { get; set; }
        public List<WorkFlowItem> WorkFlowItems { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public WorkFlowDTO ToDTO()
        {
            var dto = new WorkFlowDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.OrganizationId = OrganizationId;
            dto.ActiveFlag = ActiveFlag;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            return dto;
        }
    }

    public class WorkFlowDTO : EntityBase, IListable
    {
        public string Icon { get; set; }
        public FileAsset IconFileAsset { get; set; }
    }
}
