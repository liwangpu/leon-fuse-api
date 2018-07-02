using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 有很多ICon资源类型的Resource需要替换ICon图标
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="DTO">DTO实体对象</typeparam>
    public class ListableController<T, DTO> : CommonController<T, DTO>
               where T : class, IListable, ApiModel.ICloneable, IDTOTransfer<DTO>, new()
          where DTO : class, IData, new()
    {
        #region 构造函数
        public ListableController(IStore<T, DTO> store)
          : base(store)
        {

        }
        #endregion

        #region ChangeICon 更新图标信息
        /// <summary>
        /// 更新图标信息
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("ChangeICon")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        {
            var mapping = new Func<T, Task<T>>(async (spec) =>
            {
                spec.Icon = icon.AssetId;
                return await Task.FromResult(spec);
            });
            return await _PutRequest(icon.ObjId, mapping);
        }
        #endregion

        #region _PostCollectionRequest 处理添加收藏数据请求
        /// <summary>
        /// 处理添加收藏数据请求
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PostCollectionRequest(Func<Collection, Task<Collection>> mapping)
        {
            var accid = AuthMan.GetAccountId(this);
            var account = await _Store.DbContext.Accounts.FindAsync(accid);
            var metadata = new Collection();
            //var canCreate = await _Store.CanCreateAsync(accid);
            //if (!canCreate)
            //    return Forbid();
            var data = await mapping(metadata);
            data.Id = GuidGen.NewGUID();
            data.CreatedTime = DateTime.UtcNow;
            data.ModifiedTime = DateTime.UtcNow;
            data.Creator = accid;
            data.Modifier = accid;
            data.OrganizationId = account.OrganizationId;
            _Store.DbContext.Collections.Add(data);
            await _Store.DbContext.SaveChangesAsync();
            //await _Store.SatisfyCreateAsync(accid, data, ModelState);
            //if (!ModelState.IsValid)
            //    return new ValidationFailedResult(ModelState);
            //await _Store.CreateAsync(accid, data);
            //var dto = await _Store.GetByIdAsync(metadata.Id);
            ////如果handle不为空,由handle掌控ActionResult
            //if (handle != null)
            //    return await handle(data);
            return Ok();
        }
        #endregion

        #region CreateCollection 添加收藏数据信息
        /// <summary>
        /// 添加收藏数据信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Collection")]
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> CreateCollection([FromBody]CollectionCreateModel model)
        {
            var accid = AuthMan.GetAccountId(this);
            var t = new T();
            var colls = await _Store.DbContext.Collections.Where(x => x.Creator == accid && x.Type == t.GetType().Name && x.TargetId == model.TargetId).ToListAsync();
            if (colls.Count > 0)
                return Ok();

            var mapping = new Func<Collection, Task<Collection>>(async (entity) =>
            {
                entity.TargetId = model.TargetId;
                entity.Type = t.GetType().Name;
                return await Task.FromResult(entity);
            });
            return await _PostCollectionRequest(mapping);
        }
        #endregion

        #region DeleteCollection 删除收藏数据信息
        /// <summary>
        /// 删除收藏数据信息
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [Route("Collection")]
        [HttpDelete]
        public virtual async Task<IActionResult> DeleteCollection(string targetId)
        {
            var accid = AuthMan.GetAccountId(this);
            var t = new T();
            var colls = await _Store.DbContext.Collections.Where(x => x.Creator == accid && x.Type == t.GetType().Name && x.TargetId == targetId).ToListAsync();
            if (colls.Count == 0)
                return NotFound();
            foreach (var item in colls)
            {
                _Store.DbContext.Collections.Remove(item);
            }
            await _Store.DbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Collection 查询收藏列表数据信息
        /// <summary>
        /// 查询收藏列表数据信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Collection")]
        [HttpGet]
        [ValidateModel]
        [ProducesResponseType(typeof(PagedData<PackageDTO>), 200)]
        public async Task<IActionResult> Collection([FromQuery] ColletionRequestModel model)
        {
            var t = typeof(T);
            var accid = AuthMan.GetAccountId(this);
            var query = _Store.DbContext.Collections.Where(x => x.Creator == accid && x.ActiveFlag == AppConst.I_DataState_Active && x.Type == t.Name);
            if (!string.IsNullOrWhiteSpace(model.TargetId))
                query = query.Where(x => x.TargetId == model.TargetId);

            var dataQ = Enumerable.Empty<T>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(model.CategoryId))
            {
                var curCategoryTree = await _Store.DbContext.AssetCategoryTrees.FirstOrDefaultAsync(x => x.ObjId == model.CategoryId);
                var categoryQ = from it in _Store.DbContext.AssetCategoryTrees
                                where it.NodeType == curCategoryTree.NodeType && it.OrganizationId == curCategoryTree.OrganizationId
                                && it.LValue >= curCategoryTree.LValue && it.RValue <= curCategoryTree.RValue
                                select it.ObjId;
                var catIds = await categoryQ.ToListAsync();

                dataQ = from it in _Store.DbContext.Set<T>()
                        join cc in query on it.Id equals cc.TargetId
                        where catIds.Contains(it.CategoryId)
                        select it;
            }
            else
            {
                dataQ = from it in _Store.DbContext.Set<T>()
                        join cc in query on it.Id equals cc.TargetId
                        select it;
            }

            var res = await dataQ.OrderBy(x => x.Id).SimplePaging(model.Page, model.PageSize);
            var datas = res.Data;

            var pagedData = new PagedData<DTO>() { Data = new List<DTO>(), Page = res.Page, Size = res.Size, Total = res.Total };

            for (int ddx = datas.Count - 1; ddx >= 0; ddx--)
            {
                var curData = datas[ddx];
                if (string.IsNullOrWhiteSpace(curData.Icon))
                    curData.IconFileAsset = await _Store.DbContext.Files.FirstOrDefaultAsync(x => x.Id == curData.Icon);
                var creator = await _Store.DbContext.Accounts.FirstOrDefaultAsync(x => x.Creator == curData.Creator);
                curData.CreatorName = creator != null ? creator.Name : "";
                var modifier = await _Store.DbContext.Accounts.FirstOrDefaultAsync(x => x.Modifier == curData.Modifier);
                curData.ModifierName = modifier != null ? modifier.Name : "";
                if (!string.IsNullOrWhiteSpace(curData.CategoryId))
                {
                    var cat = await _Store.DbContext.AssetCategories.FirstOrDefaultAsync(x => x.Id == curData.CategoryId);
                    curData.CategoryName = cat != null ? cat.Name : "";
                }
                pagedData.Data.Add(curData.ToDTO());
            }

            return Ok(pagedData);
        }
        #endregion

        #region Share 分享数据信息
        /// <summary>
        /// 分享数据信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("Share")]
        [HttpPut]
        public async Task<IActionResult> Share(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return BadRequest();

            var idsArr = ids.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in idsArr)
            {
                var entity = await _Store.DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
                if (entity != null)
                {
                    entity.ResourceType = (int)ResourceTypeEnum.Organizational_SubShare;
                    entity.ModifiedTime = DateTime.UtcNow;
                    _Store.DbContext.Update<T>(entity);
                }
                await _Store.DbContext.SaveChangesAsync();
            }
            return Ok();
        } 
        #endregion
    }
}