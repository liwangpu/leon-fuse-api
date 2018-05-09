using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
namespace ApiServer.Stores
{
    public class OrderStore : PermissionStore<Order>
    {
        #region 构造函数
        public OrderStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SimplePagedQueryAsync 简单返回分页查询DTO信息
        /// <summary>
        /// 简单返回分页查询DTO信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public async Task<PagedData<OrderDTO>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Order, bool>> searchExpression = null)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Orders
                            select it;
                _OrderByPipe(ref query, orderBy, desc);
                _SearchExpressionPipe(ref query, searchExpression);
                _BasicPermissionPipe(ref query, currentAcc);
                var result = await query.SimplePaging(page, pageSize);
                if (result.Total > 0)
                    return new PagedData<OrderDTO>() { Data = result.Data.Select(x => x.ToDTO()), Total = result.Total, Page = page, Size = pageSize };
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductStore SimplePagedQueryAsync", ex);
            }
            return new PagedData<OrderDTO>();
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDTO> GetByIdAsync(string accid, string id)
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
            return data.ToDTO();
        }
        #endregion

        #region CanCreate 判断订单是否符合存储规范
        /// <summary>
        /// 判断订单是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanCreate(string accid, Order data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断订单是否符合更新规范
        /// <summary>
        /// 判断订单是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanUpdate(string accid, Order data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断订单信息是否符合删除规范
        /// <summary>
        /// 判断订单信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CanDelete(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Orders
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region CanRead 判断用户是否有权限读取该记录信息
        /// <summary>
        /// 判断用户是否有权限读取该记录信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CanRead(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Orders
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region CreateAsync 新建订单信息
        /// <summary>
        /// 新建订单信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<OrderDTO> CreateAsync(string accid, Order data)
        {
            try
            {
                data.Id = GuidGen.NewGUID();
                data.Creator = accid;
                data.Modifier = accid;
                data.AccountId = accid;
                data.CreatedTime = DateTime.Now;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Orders.Add(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("OrderStore CreateAsync", ex);
                return new OrderDTO();
            }
            return data.ToDTO();
        }
        #endregion

        #region UpdateAsync 更新订单信息
        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<OrderDTO> UpdateAsync(string accid, Order data)
        {
            try
            {
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Orders.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("OrderStore UpdateAsync", ex);
            }
            return new OrderDTO();
        }
        #endregion

        #region DeleteAsync 删除订单信息
        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string accid, string id)
        {
            try
            {
                //TODO:不是直接删除,应该active flag 为false
                var data = await _GetByIdAsync(id);
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Orders.Remove(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("OrderStore DeleteAsync", ex);
            }
        }
        #endregion

        #region Exist 判断订单信息是否存在
        /// <summary>
        /// 判断订单信息是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> Exist(string id)
        {
            //TODO:Order应该有active flag
            return await base.Exist(id);
        }
        #endregion

    }
}
