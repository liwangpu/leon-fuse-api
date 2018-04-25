﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class OrganMember : EntityBase, IEntity
    {
        public string AccountId { get; set; }

        public string OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; }
        public string DepartmentId { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }
        public DateTime JoinOrganTime { get; set; }
        public DateTime JoinDepartmentTime { get; set; }

        public string Role { get; set; }

        [JsonIgnore]
        public Account Account { get; set; }
        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = this.Id;
            dicData["Name"] = this.Name;
            return dicData;
        }
    }
}
