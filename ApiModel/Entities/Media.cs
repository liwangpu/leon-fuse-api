using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class Media : EntityBase, IListable, IDTOTransfer<MediaDTO>
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string SolutionId { get; set; }
        public string Type { get; set; }
        public List<MediaShareResource> MediaShareResources { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        [NotMapped]
        public string FileAssetUrl { get; set; }

        public MediaDTO ToDTO()
        {
            var dto = new MediaDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Rotation = Rotation;
            dto.Location = Location;
            dto.FileAssetId = FileAssetId;
            dto.SolutionId = SolutionId;
            dto.OrganizationId = OrganizationId;
            dto.Type = Type;
            if (FileAsset != null)
            {
                dto.FileAssetUrl = FileAsset.Url;
            }
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            if (MediaShareResources != null && MediaShareResources.Count > 0)
            {
                var list = new List<MediaShareResourceDTO>();
                for (int idx = MediaShareResources.Count - 1; idx >= 0; idx--)
                {
                    var curItem = MediaShareResources[idx];
                    curItem.Type = Type;
                    curItem.Rotation = Rotation;
                    curItem.Location = Location;
                    curItem.FileAssetId = FileAssetId;
                    curItem.IconFileAsset = IconFileAsset;
                    curItem.FileAsset = FileAsset;
                    list.Add(curItem.ToDTO());
                }
                dto.MediaShares = list.OrderByDescending(x => x.CreatedTime).ToList();
            }
            return dto;
        }
    }

    public class MediaShareResource : EntityBase, IListable, IDTOTransfer<MediaShareResourceDTO>
    {
        public long StartShareTimeStamp { get; set; }
        public long StopShareTimeStamp { get; set; }
        public string Password { get; set; }

        public Media Media { get; set; }
        public string MediaId { get; set; }

        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string Rotation { get; set; }
        [NotMapped]
        public string Location { get; set; }
        [NotMapped]
        public string Icon { get; set; }
        [NotMapped]
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }

        public MediaShareResourceDTO ToDTO()
        {
            var dto = new MediaShareResourceDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Type = Type;
            dto.Rotation = Rotation;
            dto.Location = Location;
            dto.StartShareTimeStamp = StartShareTimeStamp;
            dto.StopShareTimeStamp = StopShareTimeStamp;
            dto.FileAssetId = FileAssetId;
            dto.Password = Password;
            dto.Icon = Icon;
            if (FileAsset != null)
            {
                dto.FileAssetUrl = FileAsset.Url;
            }
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
            }
            return dto;
        }
    }

    public class MediaShareResourceDTO : EntityBase
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public long StartShareTimeStamp { get; set; }
        public long StopShareTimeStamp { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string FileAssetUrl { get; set; }
    }

    public class MediaDTO : EntityBase
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string IconAssetId { get; set; }
        public string FileAssetUrl { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string SolutionId { get; set; }
        public List<MediaShareResourceDTO> MediaShares { get; set; }

    }
}
