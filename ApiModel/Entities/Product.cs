﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ApiModel.Entities
{
    public class Product : EntityBase, IAsset, IDTOTransfer<ProductDTO>
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [JsonIgnore]
        public List<ProductSpec> Specifications { get; set; }
        

        public ProductDTO ToDTO()
        {
            var dto = new ProductDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.FolderId = FolderId;
            dto.CategoryId = CategoryId;
            dto.AccountId = AccountId;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            if (Specifications != null && Specifications.Count > 0)
                dto.Specifications = Specifications.Select(x => x.ToDTO()).ToList();
            return dto;
        }
    }

    public class ProductDTO : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public List<ProductSpecDTO> Specifications { get; set; }
    }

}
