using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;

namespace ApiServer.Repositories
{
    public class LayoutRepository : ListableRepository<Layout, LayoutDTO>
    {
        public LayoutRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {
        }

        /// <summary>
        /// 资源访问类型
        /// </summary>
        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational;
            }
        }
    }
}
