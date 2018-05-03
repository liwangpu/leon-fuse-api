﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Organization : EntityBase, IListable
    {
        /// <summary>
        /// 父级组织的ID
        /// </summary>
        [JsonIgnore]
        public string ParentId { get; set; }
        /// <summary>
        /// 父级组织
        /// </summary>
        [JsonIgnore]
        public Organization Parent { get; set; }

        /// <summary>
        /// 拥有者ID，反向引用
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// 拥有者，反向引用
        /// </summary>
        [JsonIgnore]
        [InverseProperty("Organization")]
        [ForeignKey("OwnerId")]
        public Account Owner { get; set; }

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
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Mail { get; set; }
        public string Location { get; set; }

    }
}
