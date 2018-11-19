using ApiModel.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Organization : EntityBase, IListable, IDTOTransfer<OrganizationDTO>
    {
        /// <summary>
        /// 父级组织的ID
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 组织类型,可能为品牌商(brand),合伙人(partner),供应商(supplier)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 父级组织
        /// </summary>
        public Organization Parent { get; set; }

        public string OwnerId { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }

        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public List<Department> Departments { get; set; }
        public List<ClientAsset> ClientAssets { get; set; }
        public List<Product> Products { get; set; }
        public List<Solution> Solutions { get; set; }
        public List<Layout> Layouts { get; set; }
        public List<Order> Orders { get; set; }
        public List<AssetFolder> Folders { get; set; }
        public List<FileAsset> Files { get; set; }

        public string Icon { get; set; }
        public string Mail { get; set; }
        public string Location { get; set; }

        public OrganizationDTO ToDTO()
        {
            var dto = new OrganizationDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Mail = Mail;
            dto.Location = Location;
            dto.ParentId = ParentId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Type = Type;
            dto.ExpireTime = ExpireTime;
            dto.ActivationTime = ActivationTime;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            switch (Type)
            {
                case AppConst.OrganType_Brand:
                    dto.TypeName = "品牌商";
                    break;
                case AppConst.OrganType_Partner:
                    dto.TypeName = "合伙人";
                    break;
                case AppConst.OrganType_Supplier:
                    dto.TypeName = "供应商";
                    break;
                default:
                    dto.TypeName = "组织";
                    break;
            }

            return dto;
        }
    }



    public class OrganizationDTO : EntityBase,IListable
    {
        public string IconAssetId { get; set; }
        public string Mail { get; set; }
        public string Location { get; set; }
        public string ParentId { get; set; }
        public string OwnerId { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string Icon { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public FileAsset IconFileAsset { get; set; }
    }
}
