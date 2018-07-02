using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class OrderRepository : ListableRepository<Order, OrderDTO>
    {
        public OrderRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
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
        public override async Task<OrderDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            if (!string.IsNullOrWhiteSpace(data.Content))
            {
                data.ContentIns = JsonConvert.DeserializeObject<OrderContent>(data.Content);
                if (data.ContentIns != null && data.ContentIns.Items != null && data.ContentIns.Items.Count > 0)
                {
                    for (int idx = data.ContentIns.Items.Count - 1; idx >= 0; idx--)
                    {
                        var cur = data.ContentIns.Items[idx];
                        if (string.IsNullOrWhiteSpace(cur.ProductSpecId))
                            continue;


                        var spec = await _DbContext.ProductSpec.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == cur.ProductSpecId);
                        if (spec != null)
                        {
                            cur.ProductSpecName = spec.Name;
                            cur.ProductName = spec.Product != null ? spec.Product.Name : "";
                            data.ContentIns.Items[idx] = cur;
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(data.Icon))
            {
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            }
            return data.ToDTO();
        }
        #endregion
    }
}
