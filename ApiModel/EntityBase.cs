using System;
using System.Collections.Generic;

namespace ApiModel
{
    public class EntityBase : IEntity, ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public EntityBase()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                CreatedTime = DateTime.Now;
                ModifiedTime = DateTime.Now;
            }
            else
            {
                ModifiedTime = DateTime.Now;
            }
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public virtual Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            return dicData;
        }
    }
}
