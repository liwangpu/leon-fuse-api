using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
namespace ApiServer.Repositories
{
    public class FileRepository : ListableRepository<FileAsset, FileAssetDTO>
    {

        public FileRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {
        }

        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational;
            }
        }
    }
}
