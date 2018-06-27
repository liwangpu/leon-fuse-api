﻿using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    public class MediaShareStore : ListableStore<MediaShareResource, MediaShareResourceDTO>, IStore<MediaShareResource, MediaShareResourceDTO>
    {
        /// <summary>
        /// 资源访问类型
        /// </summary>
        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.NoLimit;
            }
        }

        #region 构造函数
        public MediaShareStore(ApiDbContext context)
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
        public async Task SatisfyCreateAsync(string accid, MediaShareResource data, ModelStateDictionary modelState)
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
        public async Task SatisfyUpdateAsync(string accid, MediaShareResource data, ModelStateDictionary modelState)
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
        public override async Task<MediaShareResourceDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            var media = await _DbContext.Medias.FindAsync(data.MediaId);
            data.FileAssetId = media.FileAssetId;
            data.Rotation = media.Rotation;
            data.Location = media.Location;
            if (!string.IsNullOrWhiteSpace(media.Icon))
                data.IconFileAsset = await _DbContext.Files.FindAsync(media.Icon);
            if (!string.IsNullOrWhiteSpace(media.FileAssetId))
                data.FileAsset = await _DbContext.Files.FindAsync(media.FileAssetId);



            return data.ToDTO();
        }
        #endregion

        #region DeleteAsync 删除实体信息
        /// <summary>
        /// 删除实体信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(string accid, string id)
        {
            var data = await _GetByIdAsync(id);
            if (data != null)
            {
                _DbContext.MediaShareResources.Remove(data);
                await _DbContext.SaveChangesAsync();
            }
        } 
        #endregion
    }
}
