using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ApiModel.Extension;
using ApiServer.Services;

namespace ApiServer.Stores
{
    public class MediaStore : ListableStore<Media, MediaDTO>, IStore<Media, MediaDTO>
    {
        /// <summary>
        /// 资源访问类型
        /// </summary>
        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Personal;
            }
        }

        #region 构造函数
        public MediaStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Media data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
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
        public async Task SatisfyUpdateAsync(string accid, Media data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<MediaDTO> GetByIdAsync(string id)
        {
            var data = await _DbContext.Medias.Include(x => x.MediaShareResources).FirstOrDefaultAsync(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(data.Icon))
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            if (!string.IsNullOrWhiteSpace(data.FileAssetId))
                data.FileAsset = await _DbContext.Files.FindAsync(data.FileAssetId);

            data.Server = AppConfig.Instance.Configuration["MediaShareServer"];

            return data.ToDTO();
        }
        #endregion

        public override async Task CreateAsync(string accid, Media data)
        {
            await base.CreateAsync(accid, data);
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            #region 创建默认分享
            var defaultShare = new MediaShareResource();
            defaultShare.Media = data;
            defaultShare.MediaId = data.Id;
            defaultShare.Id = GuidGen.NewGUID();
            defaultShare.Name = "默认分享";
            defaultShare.StartShareTimeStamp = DateTime.UtcNow.ReferUnixTimestampFromDateTime();
            defaultShare.StopShareTimeStamp = DateTime.UtcNow.AddYears(1).ReferUnixTimestampFromDateTime();
            defaultShare.Creator = accid;
            defaultShare.Modifier = accid;
            defaultShare.OrganizationId = currentAcc.OrganizationId;
            defaultShare.CreatedTime = DateTime.UtcNow;
            defaultShare.ModifiedTime = DateTime.UtcNow;
            DbContext.MediaShareResources.Add(defaultShare);
            await DbContext.SaveChangesAsync();
            #endregion
        }
    }
}
