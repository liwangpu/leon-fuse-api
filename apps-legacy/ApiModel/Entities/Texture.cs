﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Texture : ClientAssetEntity, IListable, IDTOTransfer<TextureDTO>
    {
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }

        public TextureDTO ToDTO()
        {
            var dto = new TextureDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.PackageName = PackageName;
            dto.Description = Description;
            dto.UnCookedAssetId = UnCookedAssetId;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Dependencies = Dependencies;
            dto.Properties = Properties;
            dto.CategoryName = CategoryName;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            if (FileAsset != null)
                dto.FileAsset = FileAsset.ToDTO();

            return dto;
        }
    }

    public class TextureDTO : ClientAssetEntity
    {
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public FileAssetDTO FileAsset { get; set; }
    }
}