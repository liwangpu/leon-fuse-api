using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.MoreJee.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Repositories
{
    public class TreeBaseRepository<T> : ITreeRepository<T>
             where T : class, ITree, new()
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public TreeBaseRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        #region protected AddNewNode 添加新节点
        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task AddNewNode(T data)
        {
            data.Id = GuidGen.NewGUID();
            data.LValue = 1;
            data.RValue = 2;
            _Context.Set<T>().Add(data);
            await _Context.SaveChangesAsync();
        }
        #endregion

        #region protected AddChildNode 添加子节点
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task AddChildNode(T data)
        {
            var parentNode = await _Context.Set<T>().FindAsync(data.ParentId);
            if (parentNode != null)
            {
                data.Id = GuidGen.NewGUID();
                data.LValue = parentNode.RValue;
                data.RValue = data.LValue + 1;
                var refNodes = await _Context.Set<T>().Where(x => x.NodeType == data.NodeType && x.OrganizationId == data.OrganizationId && x.RValue >= parentNode.RValue).ToListAsync();
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
                    _Context.Set<T>().Update(cur);
                }
                //添加新节点
                _Context.Set<T>().Add(data);
                await _Context.SaveChangesAsync();
            }
        }
        #endregion

        #region GetDescendantNode 获取下级节点
        /// <summary>
        /// 获取下级节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeTypes"></param>
        /// <param name="includeCurrentNode"></param>
        /// <returns></returns>
        public async Task<IQueryable<T>> GetDescendantNode(string objId, List<string> nodeTypes, bool includeCurrentNode = false)
        {
            var node = await _Context.Set<T>().FirstAsync(x => x.ObjId == objId);
            if (includeCurrentNode)
            {
                return from it in _Context.Set<T>()
                       where it.LValue >= node.LValue && it.RValue <= node.RValue
                       && nodeTypes.Contains(it.NodeType)
                       select it;
            }
            else
            {
                return from it in _Context.Set<T>()
                       where it.LValue > node.LValue && it.RValue < node.RValue
                       && nodeTypes.Contains(it.NodeType)
                       select it;
            }
        }
        #endregion

        #region GetAncestorNode 获取上级节点
        /// <summary>
        /// 获取上级节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeTypes"></param>
        /// <param name="includeCurrentNode"></param>
        /// <returns></returns>
        public async Task<IQueryable<T>> GetAncestorNode(string objId, List<string> nodeTypes, bool includeCurrentNode = false)
        {
            var node = await _Context.Set<T>().FirstAsync(x => x.ObjId == objId);

            var ids = new List<string>();
            if (includeCurrentNode)
                ids.Add(node.Id);
            var lastNode = node;
            while (true)
            {
                if (!string.IsNullOrWhiteSpace(lastNode.ParentId))
                {
                    var parentNode = await _Context.Set<T>().FirstOrDefaultAsync(x => x.Id == lastNode.ParentId);
                    if (parentNode != null)
                    {
                        if (nodeTypes.Contains(parentNode.NodeType))
                        {
                            ids.Add(parentNode.Id);
                            lastNode = parentNode;
                        }
                    }
                    else
                        break;
                }
                else
                {
                    if (lastNode.Id != node.Id)
                    {
                        if (nodeTypes.Contains(lastNode.NodeType))
                        {
                            ids.Add(lastNode.Id);
                        }
                    }
                    break;
                }
            }

            return from it in _Context.Set<T>()
                   where ids.Contains(it.Id)
                   select it;

        }
        #endregion

        public async virtual Task CreateAsync(T data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            if (string.IsNullOrWhiteSpace(data.ParentId))
                await AddNewNode(data);
            else
                await AddChildNode(data);
        }

        public async virtual Task DeleteAsync(string id, string accountId)
        {
            var node = await _Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            /*
             * 删除节点的基础理论
             * 1.删除当前节点以及左右值包含的所有节点
             * 2.设当前节点左右值之间的差距是x
             *      树上右值都大于当前节点右值的节点中
             *          1)如果左值大于当前节点,则该节点的左右值都减去(x+1)
             *          2)如果左值小于当前节点,则仅右值减去(x+1)
             */

            //1.删除当前节点以及左右值包含的所有节点
            var containNodes = await _Context.Set<T>().Where(x => x.NodeType == node.NodeType && x.OrganizationId == node.OrganizationId && x.LValue >= node.LValue && x.RValue <= node.RValue).ToListAsync();
            _Context.Set<T>().RemoveRange(containNodes);
            await _Context.SaveChangesAsync();

            //树上右值都大于当前节点右值的节点中
            var linkedNodes = await _Context.Set<T>().Where(x => x.NodeType == node.NodeType && x.OrganizationId == node.OrganizationId && x.RValue > node.RValue).ToListAsync();

            var step = node.RValue - node.LValue + 1;

            foreach (var it in linkedNodes)
            {
                if (it.LValue > node.LValue)
                {
                    //1)如果左值大于当前节点,则该节点的左右值都减去(x+1)
                    it.LValue = it.LValue - step;
                    it.RValue = it.RValue - step;
                }
                else
                {
                    //2)如果左值小于当前节点,则仅右值减去(x + 1)
                    it.RValue = it.RValue - step;
                }
            }
            _Context.Set<T>().UpdateRange(linkedNodes);
            await _Context.SaveChangesAsync();
        }

        public async Task<T> GetNodeByObjId(string objId)
        {
            return await _Context.Set<T>().FirstOrDefaultAsync(x => x.ObjId == objId);
        }
    }
}
