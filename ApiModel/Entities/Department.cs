using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Department : EntityBase, IListable
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

        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["Description"] = Description;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            if (Members != null && Members.Count > 0)
                dicData["Members"] = Members.ToDictionary();
            return dicData;
        }
    }
}
