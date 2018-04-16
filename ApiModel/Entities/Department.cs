using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Department : IListable
    {
        public string ParentId { get; set; }
        [JsonIgnore]
        public Department Parent { get; set; }

        public string OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; }

        [JsonIgnore]
        public List<OrganMember> Members { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = this.Id;
            dicData["Name"] = this.Name;
            return dicData;
        }
    }
}
