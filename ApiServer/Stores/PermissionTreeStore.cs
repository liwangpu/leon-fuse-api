using ApiModel.Entities;
using ApiServer.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Stores
{
    public class PermissionTreeStore : TreeStore<PermissionTree>
    {
        #region 构造函数
        public PermissionTreeStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region GetOrganManageNode 获取组织所管理的节点信息
        /// <summary>
        /// 获取组织所管理的节点信息
        /// </summary>
        /// <param name="organId"></param>
        /// <param name="nodeTypes"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        public async Task<IQueryable<PermissionTree>> GetOrganManageNode(string organId, List<string> nodeTypes, bool includeSelf = false)
        {
            var organNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == organId);
            if (organNode == null)
                return Enumerable.Empty<PermissionTree>().AsQueryable();

            if (includeSelf)
            {
                return from it in _DbContext.PermissionTrees
                       where it.LValue >= organNode.LValue && it.RValue <= organNode.RValue
                       && nodeTypes.Contains(it.NodeType)
                       && it.RootOrganizationId == organNode.RootOrganizationId
                       select it;
            }
            else
            {
                return from it in _DbContext.PermissionTrees
                       where it.LValue > organNode.LValue && it.RValue < organNode.RValue
                       && nodeTypes.Contains(it.NodeType)
                       && it.RootOrganizationId == organNode.RootOrganizationId
                       select it;
            }
        }
        #endregion
    }
}
