﻿using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace ApiServer.Repositories
{
    public class StaticMeshRepository : ListableRepository<StaticMesh, StaticMeshDTO>
    {
        public StaticMeshRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {
        }

        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational_SubShare;
            }
        }

        #region _GetPermissionData 获取权限数据
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="dataOp"></param>
        /// <param name="withInActive"></param>
        /// <returns></returns>
        public override async Task<IQueryable<StaticMesh>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        {
            IQueryable<StaticMesh> query;

            var currentAcc = await _DbContext.Accounts.Select(x => new Account() { Id = x.Id, OrganizationId = x.OrganizationId, Type = x.Type }).FirstOrDefaultAsync(x => x.Id == accid);

            //数据状态
            if (withInActive)
                query = _DbContext.Set<StaticMesh>();
            else
                query = _DbContext.Set<StaticMesh>().Where(x => x.ActiveFlag == AppConst.I_DataState_Active);

            //超级管理员系列不走权限判断
            if (currentAcc.Type == AppConst.AccountType_SysAdmin || currentAcc.Type == AppConst.AccountType_SysService)
                return await Task.FromResult(query);


            if (dataOp == DataOperateEnum.Retrieve)
            {
                return query;
            }
            else
            {
                if (currentAcc.Type == AppConst.AccountType_BrandAdmin || currentAcc.Type == AppConst.AccountType_BrandMember)
                    return query;

            }

            return query.Take(0);
        }
        #endregion

        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<StaticMeshDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);

            //注意,这里有个陷阱,如果这样运行,IconFileAsset和FileAsset会一模一样,暂时没有知道原因
            //data.IconFileAsset = await _DbContext.Files.FirstOrDefaultAsync(x => x.Id == data.Icon);
            //data.FileAsset = await _DbContext.Files.FirstOrDefaultAsync(x => x.Id == data.FileAssetId);
            
            var iconFileAsset = await _DbContext.Files.FirstOrDefaultAsync(x => x.Id == data.Icon);
            var fileAsset = await _DbContext.Files.FirstOrDefaultAsync(x => x.Id == data.FileAssetId);
            data.IconFileAsset = iconFileAsset;
            data.FileAsset = fileAsset;
            //if (!string.IsNullOrWhiteSpace(data.Icon))
            //    data.IconFileAsset = await _DbContext.Files.FirstOrDefaultAsync(x=>x.Id==data.Icon);

            //if (!string.IsNullOrWhiteSpace(data.FileAssetId))
            //    data.FileAsset = await _DbContext.Files.FirstOrDefaultAsync(x => x.Id == data.FileAssetId);
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
        public override async Task<PagedData<StaticMesh>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<StaticMesh>, Task<IQueryable<StaticMesh>>> advanceQuery = null)
        {
            var result = await base.SimplePagedQueryAsync(model, accid, advanceQuery);

            if (result.Total > 0)
            {
                for (int idx = result.Data.Count - 1; idx >= 0; idx--)
                {
                    var curData = result.Data[idx];
                    if (!string.IsNullOrWhiteSpace(curData.Icon))
                        curData.IconFileAsset = await _DbContext.Files.FindAsync(curData.Icon);
                }
            }
            return result;
        }
        #endregion
    }
}
