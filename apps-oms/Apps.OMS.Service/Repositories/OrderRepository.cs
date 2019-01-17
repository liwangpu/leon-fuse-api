using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.OMS.Data.Entities;
using Apps.OMS.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.OMS.Service.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public OrderRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(Order data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(Order data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(Order data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            if (data.OrderDetails != null && data.OrderDetails.Count > 0)
            {
                data.TotalNum = data.OrderDetails.Select(x => x.Num).Sum();
                data.TotalPrice = data.OrderDetails.Select(x => x.TotalPrice).Sum();
            }
            //生成订单编号
            var beginTime = new DateTime(data.CreatedTime.Year, data.CreatedTime.Month, data.CreatedTime.Day);
            var endTime = beginTime.AddDays(1);
            var orderCount = await _Context.Orders.Where(x => x.CreatedTime >= beginTime && x.CreatedTime < endTime).CountAsync();
            data.OrderNo = beginTime.ToString("yyyyMMdd") + (orderCount + 1).ToString().PadLeft(5, '0');
            _Context.Orders.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<Order>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Order>, Task<IQueryable<Order>>> advanceQuery = null)
        {
            var query = _Context.Orders.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));
            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(Order data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            if (data.OrderDetails != null && data.OrderDetails.Count > 0)
            {
                data.TotalNum = data.OrderDetails.Select(x => x.Num).Sum();
                data.TotalPrice = data.OrderDetails.Select(x => x.TotalPrice).Sum();
            }
            _Context.Orders.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
