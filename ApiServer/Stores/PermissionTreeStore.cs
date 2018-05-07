using ApiModel.Entities;
using ApiServer.Data;

namespace ApiServer.Stores
{
    public class PermissionTreeStore : TreeStore<PermissionTree>
    {
        #region 构造函数
        public PermissionTreeStore(ApiDbContext context)
           : base(context)
        {

        }
        #endregion

    }
}
