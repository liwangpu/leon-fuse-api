using Apps.Base.Common;
using Apps.Base.Common.Consts;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Repositories
{
    public class OrganizationRepository : IRepository<Organization>
    {
        protected readonly AppDbContext _Context;
        protected readonly ITreeRepository<OrganizationTree> _OrganTreeRepository;

        #region 构造函数
        public OrganizationRepository(AppDbContext context, ITreeRepository<OrganizationTree> organTreeRepository)
        {
            _Context = context;
            _OrganTreeRepository = organTreeRepository;
        }
        #endregion

        #region CanCreateAsync
        /// <summary>
        /// CanCreateAsync
        /// </summary>
        /// <param name="data"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<string> CanCreateAsync(Organization data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDeleteAsync
        /// <summary>
        /// CanDeleteAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanGetByIdAsync
        /// <summary>
        /// CanGetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdateAsync
        /// <summary>
        /// CanUpdateAsync
        /// </summary>
        /// <param name="data"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<string> CanUpdateAsync(Organization data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CreateAsync
        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="data"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task CreateAsync(Organization data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.Organizations.Add(data);
            await _Context.SaveChangesAsync();
        }
        #endregion

        #region UpdateAsync
        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="data"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Organization data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.Organizations.Update(data);
            await _Context.SaveChangesAsync();
        }
        #endregion

        #region DeleteAsync
        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetByIdAsync
        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Organization> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.Organizations.Where(x => x.Id == id).FirstOrDefaultAsync();

            return entity;
        }
        #endregion

        #region SimplePagedQueryAsync
        /// <summary>
        /// SimplePagedQueryAsync
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="advanceQuery"></param>
        /// <returns></returns>
        public async Task<PagedData<Organization>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Organization>, Task<IQueryable<Organization>>> advanceQuery = null)
        {
            var query = _Context.Organizations.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));


            #region 组织树过滤
            {
                var currentOrganId = await _Context.Accounts.Where(x => x.Id == accountId).Select(x => x.OrganizationId).FirstAsync();
                var organTypes = await _Context.OrganizationTypes.Select(x => x.Id).ToListAsync();
                IQueryable<OrganizationTree> treeQ = null;

                //超级管理员获取根组织
                if (accountId == AppConst.BambooAdminId)
                    treeQ = _Context.OrganizationTrees.Where(x => x.LValue == 1);
                else
                    treeQ = await _OrganTreeRepository.GetDescendantNode(currentOrganId, organTypes);

                query = from it in treeQ
                        join q in query on it.ObjId equals q.Id
                        select q;
            }
            #endregion

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "ModifiedTime", model.Desc);
            return result;
        }
        #endregion

    }
}
