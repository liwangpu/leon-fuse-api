using ApiModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Models
{
    public class IconModel
    {
        public string ObjId { get; set; }
        public string AssetId { get; set; }
    }

    public class StaticMeshEditModel : IModel<StaticMesh>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FileAssetId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public StaticMesh ToEntity()
        {
            var entity = new StaticMesh();
            entity.Id = Id;
            entity.Name = Name;
            entity.Description = Description;
            entity.Icon = FileAssetId;
            //entity. = Id;
            //entity.Id = Id;

            return entity;
        }
    }
}
