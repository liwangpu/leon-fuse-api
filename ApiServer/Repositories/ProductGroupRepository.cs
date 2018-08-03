using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ApiServer.Repositories
{
    public class ProductGroupRepository : ListableRepository<ProductGroup, ProductGroupDTO>
    {
        public ProductGroupRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
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

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ProductGroupDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            if (!string.IsNullOrWhiteSpace(data.Icon))
            {
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            }
            if (!string.IsNullOrWhiteSpace(data.CategoryId))
                data.AssetCategory = await _DbContext.AssetCategories.FindAsync(data.CategoryId);
            return data.ToDTO();
        }
        #endregion

        #region override SimplePagedQueryAsync
        /// <summary>
        /// SimplePagedQueryAsync
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accid"></param>
        /// <param name="advanceQuery"></param>
        /// <returns></returns>
        public override async Task<PagedData<ProductGroup>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<ProductGroup>, Task<IQueryable<ProductGroup>>> advanceQuery = null)
        {
            var result = await base.SimplePagedQueryAsync(model, accid, advanceQuery);

            if (result.Total > 0)
            {
                for (int idx = result.Data.Count - 1; idx >= 0; idx--)
                {
                    var curData = result.Data[idx];
                    if (!string.IsNullOrWhiteSpace(curData.Icon))
                        curData.IconFileAsset = await _DbContext.Files.FindAsync(curData.Icon);

                    if (!string.IsNullOrWhiteSpace(curData.CategoryId))
                        curData.AssetCategory = await _DbContext.AssetCategories.FindAsync(curData.CategoryId);
                }
            }
            return result;
        }
        #endregion

        #region _GetPermissionData
        ///// <summary>
        ///// _GetPermissionData
        ///// </summary>
        ///// <param name="accid"></param>
        ///// <param name="dataOp"></param>
        ///// <param name="withInActive"></param>
        ///// <returns></returns>
        //public override async Task<IQueryable<ProductGroup>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        //{
        //    var query = await base._GetPermissionData(accid, dataOp, withInActive);

        //    #region 获取父组织分享的方案数据
        //    if (dataOp == DataOperateEnum.Retrieve)
        //    {
        //        var account = await _DbContext.Accounts.FindAsync(accid);
        //        var curNode = await _DbContext.PermissionTrees.FirstAsync(x => x.ObjId == account.OrganizationId);
        //        var parentOrgQ = await _PermissionTreeRepository.GetAncestorNode(curNode, new List<string>() { AppConst.S_NodeType_Organization });
        //        var parentOrgIds = await parentOrgQ.Select(x => x.ObjId).ToListAsync();
        //        var shareDataQ = _DbContext.ProductGroups.Where(x => parentOrgIds.Contains(x.OrganizationId) && x.ActiveFlag == AppConst.I_DataState_Active && x.ResourceType == (int)ResourceTypeEnum.Organizational_SubShare);
        //        query = query.Union(shareDataQ);
        //    }
        //    #endregion

        //    return query;
        //}
        #endregion
    }
}
