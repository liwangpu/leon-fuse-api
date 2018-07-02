using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Services;
using Microsoft.Extensions.Options;


namespace ApiServer.Repositories
{
    public class MapRepository : ListableRepository<Map, MapDTO>
    {
        //private AppConfig2 appConfig;

        //public MapRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep, IOptions<AppConfig2> settingsOptions)
        //    : base(context, permissionTreeRep)
        //{
        //    appConfig = settingsOptions.Value;

        //}
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
