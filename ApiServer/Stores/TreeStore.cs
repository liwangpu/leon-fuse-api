using ApiModel;
using ApiServer.Data;
using BambooCommon;
using Microsoft.EntityFrameworkCore;
using System;
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
            _DbContext = context ;
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
            try
            {
                data.LValue = 1;
                data.RValue = 2;
                _DbContext.Set<T>().Add(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("PermissionTreeStore AddNewNode", ex);
            }
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
            try
            {
                var parentNode = await _DbContext.Set<T>().FindAsync(data.ParentId);
                if (parentNode != null)
                {
                    data.LValue = parentNode.RValue;
                    data.RValue = data.LValue + 1;
                    var refNodes = await _DbContext.Set<T>().Where(x => x.OrganizationId == data.OrganizationId && x.RValue >= parentNode.RValue).ToListAsync();
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
            catch (Exception ex)
            {
                Logger.LogError("PermissionTreeStore AddChildNode", ex);
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
            try
            {
                var siblingNode = await _DbContext.Set<T>().FindAsync(sibling);
                if (siblingNode != null)
                {

                    data.LValue = siblingNode.RValue + 1;
                    data.RValue = data.LValue + 1;
                    var refNodes = await _DbContext.Set<T>().Where(x => x.OrganizationId == data.OrganizationId && x.RValue >= siblingNode.RValue).ToListAsync();
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
            catch (Exception ex)
            {
                Logger.LogError("PermissionTreeStore AddSiblingNode", ex);
            }
        }
        #endregion

        public async Task MoveNode(T data, string newParentNodeId)
        {
            //TODO:
        }
    }
}
