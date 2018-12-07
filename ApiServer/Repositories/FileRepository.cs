using System.Linq;
using System.Threading.Tasks;
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

        //public async override Task<IQueryable<FileAsset>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        //{
        //    var query = _DbContext.Files;
        //    return await Task.FromResult(query);
        //}
    }
}
