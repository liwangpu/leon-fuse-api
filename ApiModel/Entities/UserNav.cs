using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class UserNav : EntityBase, IListable, IDTOTransfer<UserNavDTO>
    {
        public string Role { get; set; }
        public List<UserNavDetail> UserNavDetails { get; set; }
        [NotMapped]
        public string Icon { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public UserNavDTO ToDTO()
        {
            var dto = new UserNavDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Role = Role;
            //if (RefNavigation != null)
            //{
            //    dto.RefNavigationId = RefNavigationId;
            //    dto.Title = RefNavigation.Title;
            //    dto.Icon = RefNavigation.Icon;
            //    dto.Resource = RefNavigation.Resource;
            //    dto.NodeType = RefNavigation.NodeType;

            //    if (!string.IsNullOrWhiteSpace(Field))
            //    {
            //        var excludeArr = Field.Split(",");
            //        var fullArr = RefNavigation.Field.Split(",");
            //        var destArr = fullArr.Where(x => !excludeArr.Contains(x)).ToList();
            //        Field = string.Join(',', destArr);
            //    }
            //    else
            //    {
            //        dto.Field = RefNavigation.Field;
            //    }

            //    if (!string.IsNullOrWhiteSpace(Permission))
            //    {
            //        var excludeArr = Permission.Split(",");
            //        var fullArr = RefNavigation.Permission.Split(",");
            //        var destArr = fullArr.Where(x => !excludeArr.Contains(x)).ToList();
            //        Permission = string.Join(',', destArr);
            //    }
            //    else
            //    {
            //        dto.Permission = RefNavigation.Permission;
            //    }

            //    if (!string.IsNullOrWhiteSpace(PagedModel))
            //    {
            //        var excludeArr = PagedModel.Split(",");
            //        var fullArr = RefNavigation.PagedModel.Split(",");
            //        var destArr = fullArr.Where(x => !excludeArr.Contains(x)).ToList();
            //        PagedModel = string.Join(',', destArr);
            //    }
            //    else
            //    {
            //        dto.PagedModel = RefNavigation.PagedModel;
            //    }

            //}
            return dto;
        }
    }


    public class UserNavDTO : EntityBase, IListable
    {
        public string Role { get; set; }
        public FileAsset IconFileAsset { get; set; }
        public string Icon { get; set; }
    }
}
