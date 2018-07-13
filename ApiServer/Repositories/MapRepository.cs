﻿using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;


namespace ApiServer.Repositories
{
    public class MapRepository : ListableRepository<Map, MapDTO>
    {

        /// <summary>
        /// 资源访问类型
        /// </summary>
        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational_SubShare;
            }
        }

        public MapRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {

        }


        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        ///// <summary>
        ///// 根据Id返回实体DTO数据信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public override async Task<MapDTO> GetByIdAsync(string id)
        //{
        //    var data = await _GetByIdAsync(id);

        //    if (!string.IsNullOrWhiteSpace(data.Icon))
        //        data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
        //    if (!string.IsNullOrWhiteSpace(data.FileAssetId))
        //        data.FileAsset = await _DbContext.Files.FindAsync(data.FileAssetId);
        //    return data.ToDTO();
        //}
        #endregion

    }
}