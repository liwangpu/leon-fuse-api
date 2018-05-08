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

namespace ApiServer.Stores
{
    /// <summary>
    /// Material Store
    /// </summary>
    public class MaterialStore : PermissionStore<Material>
    {
        #region 构造函数
        public MaterialStore(ApiDbContext context)
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
        public async Task<PagedData<MaterialDTO>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Material, bool>> searchExpression = null)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Materials
                            select it;
                _OrderByPipe(ref query, orderBy, desc);
                _SearchExpressionPipe(ref query, searchExpression);
                _BasicPermissionPipe(ref query, currentAcc);
                var result = await query.SimplePaging(page, pageSize);
                if (result.Total > 0)
                    return new PagedData<MaterialDTO>() { Data = result.Data.Select(x => x.ToDTO()), Total = result.Total, Page = page, Size = pageSize };
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductStore SimplePagedQueryAsync", ex);
            }
            return new PagedData<MaterialDTO>();
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaterialDTO> GetByIdAsync(string accid, string id)
        {
            var data = await _GetByIdAsync(id);
            return data.ToDTO();
        }
        #endregion

        #region CanCreate 判断材料是否符合存储规范
        /// <summary>
        /// 判断材料是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanCreate(string accid, Material data, ModelStateDictionary modelState)
        {
           await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断材料是否符合更新规范
        /// <summary>
        /// 判断材料是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanUpdate(string accid, Material data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断材料信息是否符合删除规范
        /// <summary>
        /// 判断材料信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CanDelete(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Materials
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
            var query = from it in _DbContext.Materials
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region CreateAsync 新建材质信息
        /// <summary>
        /// 新建材质信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<MaterialDTO> CreateAsync(string accid, Material data)
        {
            try
            {
                data.Id = GuidGen.NewGUID();
                data.Creator = accid;
                data.Modifier = accid;
                data.CreatedTime = DateTime.Now;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Materials.Add(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("MaterialStore CreateAsync", ex);
                return new MaterialDTO();
            }
            return data.ToDTO();
        }
        #endregion

        #region UpdateAsync 更新材质信息
        /// <summary>
        /// 更新材质信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<MaterialDTO> UpdateAsync(string accid, Material data)
        {
            try
            {
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Materials.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("MaterialStore UpdateAsync", ex);
            }
            return new MaterialDTO();
        }
        #endregion

        #region DeleteAsync 删除材质信息
        /// <summary>
        /// 删除材质信息
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
                _DbContext.Materials.Remove(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("MaterialStore DeleteAsync", ex);
            }
        }
        #endregion

        #region Exist 判断材质信息是否存在
        /// <summary>
        /// 判断材质信息是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> Exist(string id)
        {
            //TODO:Material应该有active flag
            return await base.Exist(id);
        }
        #endregion
    }
}
