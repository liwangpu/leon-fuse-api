using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
            dto.Rotation = Rotation;
            dto.Location = Location;
            dto.FileAssetId = FileAssetId;
            dto.SolutionId = SolutionId;
            dto.OrganizationId = OrganizationId;
            dto.Type = Type;
            if (IconFileAsset != null)
                dto.Icon = IconFileAsset.Url;

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
        public string Rotation { get; set; }
        [NotMapped]
        public string Location { get; set; }
        [NotMapped]
        public string Icon { get; set; }
        [NotMapped]
        public string FileAssetId { get; set; }


        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

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
            dto.Rotation = Rotation;
            dto.Location = Location;
            dto.StartShareTimeStamp = StartShareTimeStamp;
            dto.StopShareTimeStamp = StopShareTimeStamp;
            dto.FileAssetId = FileAssetId;
            if (IconFileAsset != null)
                dto.Icon = IconFileAsset.Url;

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
    }

    public class MediaDTO : EntityBase
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string SolutionId { get; set; }
    }
}
