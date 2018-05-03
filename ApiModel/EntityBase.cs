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
        public string Creator { get; set; }
        public string Modifier { get; set; }

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

        public bool IsPersistence()
        {
            if (!string.IsNullOrWhiteSpace(Id))
                return true;
            return false;
        }
    }
}
