﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Solution : EntityBase, IAsset, IDTOTransfer<SolutionDTO>
    {
        public string Icon { get; set; }
        public string LayoutId { get; set; }
        [JsonIgnore]
        public Layout Layout { get; set; }

        /// <summary>
        /// 内容为 SolutionData 对象的 json字符串
        /// </summary>
        public string Data { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }


        public SolutionDTO ToDTO()
        {
            var dto = new SolutionDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.FolderId = FolderId;
            dto.CategoryId = CategoryId;
            dto.AccountId = AccountId;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Data = Data;
            dto.LayoutId = LayoutId;
            return dto;
        }
    }

    public class SolutionDTO : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string Data { get; set; }
        public string LayoutId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
