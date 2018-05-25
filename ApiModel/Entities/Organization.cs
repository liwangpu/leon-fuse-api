using Newtonsoft.Json;
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

        /// <summary>
        /// 拥有者ID，反向引用
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// 拥有者，反向引用
        /// </summary>
        [InverseProperty("Organization")]
        [ForeignKey("OwnerId")]
        public Account Owner { get; set; }

        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [JsonIgnore]
        public List<Department> Departments { get; set; }

        [JsonIgnore]
        public List<ClientAsset> ClientAssets { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
        [JsonIgnore]
        public List<Solution> Solutions { get; set; }
        [JsonIgnore]
        public List<Layout> Layouts { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; }
        [JsonIgnore]
        public List<AssetFolder> Folders { get; set; }
        [JsonIgnore]
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
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Type = Type;
            return dto;
        }
    }



    public class OrganizationDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Mail { get; set; }
        public string Location { get; set; }
        public string ParentId { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Type { get; set; }
    }
}
