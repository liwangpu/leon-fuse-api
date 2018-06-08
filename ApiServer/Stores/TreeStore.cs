using ApiModel;
using ApiServer.Data;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiServer.Stores
{
    public class TreeStore<T>
              where T : class, ITree, new()
    {
        protected readonly ApiDbContext _DbContext;
        #region 构造函数
        public TreeStore(ApiDbContext context)
        {
            _DbContext = context;
        }
        #endregion

        #region AddNewNode 添加新节点
        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddNewNode(T data)
        {
            data.Id = GuidGen.NewGUID();
            data.LValue = 1;
            data.RValue = 2;
            data.RootOrganizationId = data.OrganizationId;
            _DbContext.Set<T>().Add(data);
            await _DbContext.SaveChangesAsync();
        }
        #endregion

        #region AddChildNode 添加子节点
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddChildNode(T data)
        {
            var parentNode = await _DbContext.Set<T>().FindAsync(data.ParentId);
            if (parentNode != null)
            {
                data.Id = GuidGen.NewGUID();
                data.LValue = parentNode.RValue;
                data.RValue = data.LValue + 1;
                data.RootOrganizationId = parentNode.RootOrganizationId;
                var refNodes = await _DbContext.Set<T>().Where(x => x.RootOrganizationId == data.RootOrganizationId && x.RValue >= parentNode.RValue).ToListAsync();
                for (int idx = refNodes.Count - 1; idx >= 0; idx--)
                {
                    var cur = refNodes[idx];
                    //支线上只改变右值
                    if (cur.LValue <= parentNode.LValue)
                    {
                        cur.RValue += 2;
                    }
                    else
                    {
                        cur.LValue += 2;
                        cur.RValue += 2;
                    }
                    _DbContext.Set<T>().Update(cur);
                }
                //添加新节点
                _DbContext.Set<T>().Add(data);
                await _DbContext.SaveChangesAsync();
            }
        }
        #endregion

        #region AddSiblingNode 添加相邻节点
        /// <summary>
        /// 添加相邻节点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sibling"></param>
        /// <returns></returns>
        public async Task AddSiblingNode(T data, string sibling)
        {
            var siblingNode = await _DbContext.Set<T>().FindAsync(sibling);
            if (siblingNode != null)
            {
                data.Id = GuidGen.NewGUID();
                data.LValue = siblingNode.RValue + 1;
                data.RValue = data.LValue + 1;
                data.RootOrganizationId = siblingNode.RootOrganizationId;
                var refNodes = await _DbContext.Set<T>().Where(x => x.RootOrganizationId == data.RootOrganizationId && x.RValue >= siblingNode.RValue).ToListAsync();
                for (int idx = refNodes.Count - 1; idx >= 0; idx--)
                {
                    var cur = refNodes[idx];
                    cur.RValue += 2;
                    _DbContext.Set<T>().Update(cur);
                }
                _DbContext.Set<T>().Add(data);
                await _DbContext.SaveChangesAsync();
            }
        }
        #endregion

        public IQueryable<T> GetNode(T node, List<string> nodeTypes, bool includeCurrentNode = false)
        {
            if (includeCurrentNode)
            {
                return from it in _DbContext.Set<T>()
                       where it.RootOrganizationId == node.RootOrganizationId
                       && it.LValue >= node.LValue && it.RValue <= node.RValue
                       && nodeTypes.Contains(it.NodeType)
                       select it;
            }
            else
            {
                return from it in _DbContext.Set<T>()
                       where it.RootOrganizationId == node.RootOrganizationId
                       && it.LValue > node.LValue && it.RValue < node.RValue
                       && nodeTypes.Contains(it.NodeType)
                       select it;
            }
        }

        public async Task MoveNode(T data, string newParentNodeId)
        {
            //TODO:
            await Task.FromResult(string.Empty);
        }
    }
}
