﻿using ApiModel;
using ApiModel.Enums;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 通用RestFull CRUD常用请求处理控制器
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="DTO">DTO实体对象</typeparam>
    public class CommonController<T, DTO> : Controller
         where T : class, IEntity, IDTOTransfer<DTO>, new()
           where DTO : class, IData, new()
    {
        protected bool RequestValid;
        protected IStore<T, DTO> _Store;

        #region 构造函数
        public CommonController(IStore<T, DTO> store)
        {
            _Store = store;
        }
        #endregion

        #region _GetPagingRequest 根据查询参数获取分页信息
        /// <summary>
        /// 根据查询参数获取分页信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="qMapping"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _GetPagingRequest(PagingRequestModel model, Action<List<string>> qMapping = null, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var accid = AuthMan.GetAccountId(this);
            var qs = new List<string>();
            qMapping?.Invoke(qs);
            if (qs.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (var item in qs)
                    builder.AppendFormat(";{0}", item);
                model.Q = builder.ToString();
            }
            var result = await _Store.SimplePagedQueryAsync(model, accid, resType);
            return Ok(StoreBase<T, DTO>.PageQueryDTOTransfer(result));
        }
        #endregion

        #region _GetByIdRequest 根据id获取信息
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _GetByIdRequest(string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();
            var canRead = await _Store.CanReadAsync(accid, id, resType);
            if (!canRead)
                return Forbid();
            var dto = await _Store.GetByIdAsync(id);
            return Ok(dto);
        }
        #endregion

        #region _PostRequest 处理Post请求信息
        /// <summary>
        /// 处理Post请求信息
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="handle"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PostRequest(Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> handle = null, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var accid = AuthMan.GetAccountId(this);
            var metadata = new T();
            var data = await mapping(metadata);
            metadata.Id = GuidGen.NewGUID();
            metadata.Creator = accid;
            metadata.Modifier = accid;
            metadata.CreatedTime = DateTime.Now;
            metadata.ModifiedTime = DateTime.Now;
            await _Store.SatisfyCreateAsync(accid, data, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            var canCreate = await _Store.CanCreateAsync(accid, resType);
            if (!canCreate)
                return Forbid();
            //如果handle不为空,由handle掌控Create流程和ActionResult
            if (handle != null)
                return await handle(data);
            await _Store.CreateAsync(accid, data);
            var dto = await _Store.GetByIdAsync(metadata.Id);
            return Ok(dto);
        }
        #endregion

        #region _PutRequest 处理Put请求
        /// <summary>
        /// 处理Put请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mapping"></param>
        /// <param name="handle"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PutRequest(string id, Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> handle = null, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();
            var accid = AuthMan.GetAccountId(this);
            var permission = await _Store.CanUpdateAsync(accid, id, resType);
            if (!permission)
                return Forbid();
            var metadata = await _Store._GetByIdAsync(id);
            var entity = await mapping(metadata);
            metadata.Id = id;
            metadata.Modifier = accid;
            metadata.ModifiedTime = DateTime.Now;

            await _Store.SatisfyUpdateAsync(accid, entity, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            //如果handle不为空,由handle掌控Update流程和ActionResult
            if (handle != null)
                return await handle(entity);
            await _Store.UpdateAsync(accid, entity);
            var dto = await _Store.GetByIdAsync(entity.Id);
            return Ok(dto);
        }
        #endregion

        #region _DeleteRequest 处理Delete请求
        /// <summary>
        /// 处理Delete请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _DeleteRequest(string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();

            var canDelete = await _Store.CanDeleteAsync(accid, id, resType);
            if (!canDelete)
                return Forbid();
            await _Store.DeleteAsync(accid, id);
            return Ok();
        }
        #endregion

        /**************** common method ****************/

        #region Delete 删除数据信息
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region KeyWordSearchQ 基本关键词查询
        /// <summary>
        /// 基本关键词查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<string> KeyWordSearchQ(string keyword)
        {
            var list = new List<string>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list.Add(string.Format("Name like {0}", keyword));
                //list.Add(string.Format("Description={0}", keyword));
            }
            return list;
        }
        #endregion
    }
}



