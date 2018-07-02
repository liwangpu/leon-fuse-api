using ApiModel.Entities;
using ApiServer.Data;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class TextureRepository : ListableRepository<Texture, TextureDTO>
    {
        public TextureRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep) : base(context, permissionTreeRep)
        {
        }

        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<TextureDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(data.Icon))
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            if (!string.IsNullOrWhiteSpace(data.FileAssetId))
                data.FileAsset = await _DbContext.Files.FindAsync(data.Icon);
            return data.ToDTO();
        }
        #endregion
    }
}
