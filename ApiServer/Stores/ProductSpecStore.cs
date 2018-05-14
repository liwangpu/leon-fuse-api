using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class ProductSpecStore : StoreBase<ProductSpec, ProductSpecDTO>, IStore<ProductSpec, ProductSpecDTO>
    {
        #region 构造函数
        public ProductSpecStore(ApiDbContext context)
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
        public async Task SatisfyCreateAsync(string accid, ProductSpec data, ModelStateDictionary modelState)
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
        public async Task SatisfyUpdateAsync(string accid, ProductSpec data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ProductSpecDTO> GetByIdAsync(string id)
        {
            var res = await _GetByIdAsync(id);
            if (!string.IsNullOrWhiteSpace(res.Icon))
            {
                var ass = await _DbContext.Files.FindAsync(res.Icon);
                if (ass != null)
                    res.IconFileAsset = ass;
            }
            if (!string.IsNullOrWhiteSpace(res.Album))
            {
                var albumIds = res.Album.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = albumIds.Count - 1; idx >= 0; idx--)
                {
                    var ass = await _DbContext.Files.FindAsync(albumIds[idx]);
                    if (ass != null)
                        res.AlbumAsset.Add(ass);
                }
            }
            if (!string.IsNullOrWhiteSpace(res.StaticMeshIds))
            {
                var map = JsonConvert.DeserializeObject<SpecMeshMap>(res.StaticMeshIds);
                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
                {
                    var refMesh = await _DbContext.StaticMeshs.FindAsync(map.Items[idx].StaticMeshId);
                    if (refMesh != null)
                    {
                        var tmp = await _DbContext.Files.FindAsync(refMesh.FileAssetId);
                        if (tmp != null)
                            refMesh.FileAsset = tmp;
                    }

                    if (map.Items[idx].MaterialIds != null && map.Items[idx].MaterialIds.Count > 0)
                    {
                        var matids = map.Items[idx].MaterialIds;
                        foreach (var item in matids)
                        {
                            var refMat = await _DbContext.Materials.FindAsync(item);
                            if (refMat != null)
                            {
                                var tmp = await _DbContext.Files.FindAsync(refMat.FileAssetId);
                                if (tmp != null)
                                {
                                    refMat.FileAsset = tmp;
                                    refMesh.Materials.Add(refMat);
                                }
                            }
                        }
                    }
                    res.StaticMeshAsset.Add(refMesh);
                }
            }

            return res.ToDTO();
        }
        #endregion
    }
}
