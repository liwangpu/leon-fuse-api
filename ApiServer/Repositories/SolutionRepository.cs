using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ApiServer.Repositories
{
    public class SolutionRepository : ResourceRepositoryBase<Solution, SolutionDTO>
    {
        public SolutionRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
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
        public override int ResType => ResourceTypeConst.Solution;

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async override Task SatisfyCreateAsync(string accid, Solution data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrEmpty(data.LayoutId))
            {
                var exist = await _DbContext.Layouts.CountAsync(x => x.Id == data.LayoutId && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
                if (!exist)
                    modelState.AddModelError("LayoutId", "没有找到该记录信息");
            }
        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async override Task SatisfyUpdateAsync(string accid, Solution data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrEmpty(data.LayoutId))
            {
                var exist = await _DbContext.Layouts.CountAsync(x => x.Id == data.LayoutId && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
                if (!exist)
                    modelState.AddModelError("LayoutId", "没有找到该记录信息");
            }
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
        //public override async Task<IQueryable<Solution>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        //{
        //    var query = await base._GetPermissionData(accid, dataOp, withInActive);

        //    #region 获取父组织分享的方案数据
        //    if (dataOp == DataOperateEnum.Retrieve)
        //    {
        //        var account = await _DbContext.Accounts.FindAsync(accid);
        //        var curNode = await _DbContext.PermissionTrees.FirstAsync(x => x.ObjId == account.OrganizationId);
        //        var parentOrgQ = await _PermissionTreeRepository.GetAncestorNode(curNode, new List<string>() { AppConst.S_NodeType_Organization });
        //        var parentOrgIds = await parentOrgQ.Select(x => x.ObjId).ToListAsync();
        //        var shareDataQ = _DbContext.Solutions.Where(x => parentOrgIds.Contains(x.OrganizationId) && x.ActiveFlag == AppConst.I_DataState_Active && x.ResourceType == (int)ResourceTypeEnum.Organizational_SubShare);
        //        //var shareData = await shareDataQ.ToListAsync();
        //        query = query.Union(shareDataQ);
        //    }
        //    #endregion

        //    return query;
        //}
        #endregion

        #region PagedSelectExpression
        /// <summary>
        /// PagedSelectExpression
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<Solution, Solution>> PagedSelectExpression()
        {
            return x => new Solution()
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                Description = x.Description,
                CategoryId = x.CategoryId,
                OrganizationId = x.OrganizationId,
                Creator = x.Creator,
                Modifier = x.Modifier,
                CreatedTime = x.CreatedTime,
                ModifiedTime = x.ModifiedTime,
                LayoutId = x.LayoutId,
                ResourceType = x.ResourceType
            };
        }
        #endregion

    }
}
