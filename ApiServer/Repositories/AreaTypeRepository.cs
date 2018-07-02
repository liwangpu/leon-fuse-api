using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
namespace ApiServer.Repositories
{
    public class AreaTypeRepository : ListableRepository<AreaType, AreaTypeDTO>
    {
        public AreaTypeRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
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
