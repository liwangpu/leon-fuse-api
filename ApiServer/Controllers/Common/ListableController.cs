using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
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
               where T : class, IListable, IDTOTransfer<DTO>, new()
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
            var mapping = new Func<Collection, Task<Collection>>(async (entity) =>
            {
                var t = new T();
                entity.TargetId = model.TargetId;
                entity.Folder = model.FolderName;
                entity.Type = t.GetType().Name;
                return await Task.FromResult(entity);
            });
            return await _PostCollectionRequest(mapping);
        }
        #endregion

        #region Delete 删除收藏数据信息
        /// <summary>
        /// 删除收藏数据信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Collection")]
        [HttpDelete]
        [ValidateModel]
        public virtual async Task<IActionResult> DeleteCollection([FromBody]CollectionCreateModel model)
        {
            var accid = AuthMan.GetAccountId(this);
            var t = new T();
            var colls = _Store.DbContext.Collections.Where(x => x.Creator == accid && x.Type == t.GetType().Name && x.Folder == model.FolderName && x.TargetId == model.TargetId);
            foreach (var item in colls)
            {
                _Store.DbContext.Collections.Remove(item);
            }
            await _Store.DbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        [Route("Collection")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<PackageDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] ColletionRequestModel model)
        {
            var accid = AuthMan.GetAccountId(this);
            var t = new T();

            var referCollections = new List<Collection>();

            var advanceQuery = new Func<IQueryable<T>, Task<IQueryable<T>>>(async (query) =>
            {
                var collQ = _Store.DbContext.Collections.Where(x => x.Creator == accid && x.Type == t.GetType().Name);
                if (!string.IsNullOrWhiteSpace(model.FolderName))
                {
                    collQ = collQ.Where(x => x.Folder == model.FolderName);
                }
                else
                {
                    if (model.IsInFolder)
                        collQ = collQ.Where(x => x.Folder == model.FolderName);
                }
                if (!string.IsNullOrWhiteSpace(model.TargetId))
                    collQ = collQ.Where(x => x.TargetId == model.TargetId);

                query = from it in query
                        join col in collQ on it.Id equals col.TargetId
                        select it;

                var referIds = await query.Select(x => x.Id).ToListAsync();
                referCollections = await collQ.Where(x => referIds.Contains(x.TargetId)).ToListAsync();
                return await Task.FromResult(query);
            });

            var ind = 0;
            var literal = new Func<T, IList<T>, Task<T>>(async (entity, datas) =>
             {
                 //var 
                 //entity.Content = null;
                 //entity.FolderName =

                 //查看当前T在Datas中的下标,匹配出该T在referCollections相应下标的收藏信息
                 //var sameCollections = datas.Where(x => x.Id == entity.Id);
                 //entity.GetHashCode
                 //for (int idx = referCollections.Count - 1; idx >= 0; idx--)
                 //{
                 //    var curCol = referCollections[idx];
                 //    if (curCol.TargetId == entity.Id)
                 //    {
                 //        entity.FolderName = curCol.Folder;
                 //        referCollections.RemoveAt(idx);
                 //        break;
                 //    }
                 //}

                 var aaaa = ind++;
                 entity.FolderName =Guid.NewGuid().ToString();

                 return entity;
             });
            return await _GetPagingRequest(model, null, advanceQuery, literal);
        }
    }
}