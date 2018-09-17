using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Services;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class OrderRepository : ListableRepository<Order, OrderDTO>
    {
        public AppConfig appConfig { get; }

        #region 构造函数
        public OrderRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep, IOptions<AppConfig> settingsOptions)
    : base(context, permissionTreeRep)
        {
            appConfig = settingsOptions.Value;
        }
        #endregion

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
            var data = await _DbContext.Orders.Include(x => x.OrderDetails).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data.OrderDetails != null && data.OrderDetails.Count > 0)
            {
                for (int idx = data.OrderDetails.Count - 1; idx >= 0; idx--)
                {
                    var item = data.OrderDetails[idx];
                    item.ProductSpec = await _DbContext.ProductSpec.Where(x => x.Id == item.ProductSpecId).Select(x => new ProductSpec() { Name = x.Name, ProductId = x.ProductId, Icon = x.Icon }).FirstOrDefaultAsync();
                    if (item.ProductSpec != null)
                    {
                        item.ProductSpec.Product = await _DbContext.Products.Where(x => x.Id == item.ProductSpec.ProductId).Select(x => new Product() { Name = x.Name }).FirstOrDefaultAsync();

                        if (!string.IsNullOrWhiteSpace(item.ProductSpec.Icon))
                        {
                            item.ProductSpec.IconFileAsset = await _DbContext.Files.FindAsync(item.ProductSpec.Icon);
                            //以第一个产品作为订单icon
                            if (idx == 0)
                                data.IconFileAsset = item.ProductSpec.IconFileAsset;
                        }
                    }
                }
            }
            data.Url = appConfig.Plugins.OrderViewer + "?order=" + data.Id;
            data.CreatorName = await _DbContext.Accounts.Where(x => x.Id == data.Creator).Select(x => x.Name).FirstOrDefaultAsync();
            data.ModifierName = await _DbContext.Accounts.Where(x => x.Id == data.Modifier).Select(x => x.Name).FirstOrDefaultAsync();
            return data.ToDTO();
        }
        #endregion

        #region CreateAsync 创建订单
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override async Task CreateAsync(string accid, Order data)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            data.Id = GuidGen.NewGUID();
            data.Creator = accid;
            data.Modifier = accid;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            data.OrganizationId = currentAcc.OrganizationId;
            data.Url = appConfig.Plugins.OrderViewer + "?order=" + data.Id;
            if (data.OrderDetails != null && data.OrderDetails.Count > 0)
            {
                for (int idx = data.OrderDetails.Count - 1; idx >= 0; idx--)
                {
                    var item = data.OrderDetails[idx];
                    item.Id = GuidGen.NewGUID();
                    item.Creator = accid;
                    item.Modifier = accid;
                    item.CreatedTime = data.CreatedTime;
                    item.ModifiedTime = data.CreatedTime;
                    item.OrganizationId = currentAcc.OrganizationId;
                    item.OrderDetailStateId = (int)OrderDetailStateEnum.Confirm;
                    item.ProductSpec = await _DbContext.ProductSpec.Where(x => x.Id == item.ProductSpecId).Select(x => new ProductSpec() { Name = x.Name, ProductId = x.ProductId }).FirstOrDefaultAsync();
                    if (item.ProductSpec != null)
                        item.ProductSpec.Product = await _DbContext.Products.Where(x => x.Id == item.ProductSpec.ProductId).Select(x => new Product() { Name = x.Name }).FirstOrDefaultAsync();

                }
            }
            _DbContext.Orders.Add(data);
            await _DbContext.SaveChangesAsync();
        }
        #endregion
    }
}
