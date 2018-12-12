using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Controllers
{
    public abstract class ServiceBaseController<T> : AppBaseController
        where T : class, IData, new()
    {
        protected readonly IRepository<T> _Repository;

        public abstract Task<IActionResult> Get(string id);

        #region 构造函数
        public ServiceBaseController(IRepository<T> repository)
        {
            _Repository = repository;
        }
        #endregion

        #region _PagingRequest 处理分页请求
        /// <summary>
        /// 处理分页请求
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="model"></param>
        /// <param name="toDTO"></param>
        /// <param name="advanceQuery"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PagingRequest<DTO>([FromQuery]PagingRequestModel model, Func<T, Task<DTO>> toDTO, Func<IQueryable<T>, Task<IQueryable<T>>> advanceQuery = null)
              where DTO : class, new()
        {
            var result = new PagedData<DTO>();
            var res = await _Repository.SimplePagedQueryAsync(model, CurrentAccountId, advanceQuery);
            result.Page = res.Page;
            result.Size = res.Size;
            result.Total = res.Total;
            var dtos = new List<DTO>();
            if (res.Data != null)
            {
                foreach (var item in res.Data)
                {
                    var dto = await toDTO(item);
                    dtos.Add(dto);
                }
            }
            result.Data = dtos;
            return Ok(result);
        }
        #endregion

        #region _GetByIdRequest 处理Get请求信息
        /// <summary>
        /// 处理Get请求信息
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="id"></param>
        /// <param name="toDTO"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _GetByIdRequest<DTO>(string id, Func<T, Task<DTO>> toDTO)
               where DTO : class, new()
        {
            var canReadMessage = await _Repository.CanGetByIdAsync(id, CurrentAccountId);
            if (!string.IsNullOrWhiteSpace(canReadMessage))
            {
                ModelState.AddModelError("message", canReadMessage);
                return BadRequest(ModelState);
            }
            var entity = await _Repository.GetByIdAsync(id, CurrentAccountId);
            if (entity == null)
                return NotFound();
            var dto = await toDTO(entity);
            return Ok(dto);
        }
        #endregion

        #region _PostRequest 处理Post请求信息
        /// <summary>
        /// 处理Post请求信息
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="afterCreated"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PostRequest(Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> afterCreated = null)
        {
            var data = await mapping(new T());
            var createdMessage = await _Repository.CanCreateAsync(data, CurrentAccountId);
            if (!string.IsNullOrWhiteSpace(createdMessage))
            {
                ModelState.AddModelError("message", createdMessage);
                return BadRequest(ModelState);
            }
            await _Repository.CreateAsync(data, CurrentAccountId);
            if (afterCreated != null)
                return await afterCreated(data);
            return await Get(data.Id);
        }
        #endregion

        #region _PutRequest 处理Update请求
        /// <summary>
        /// 处理Update请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mapping"></param>
        /// <param name="afterUpdated"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PutRequest(string id, Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> afterUpdated = null)
        {
            var metadata = await _Repository.GetByIdAsync(id, CurrentAccountId);
            if (metadata == null)
                return NotFound();
            var data = await mapping(metadata);
            //为了防止Id不小心被改动
            data.Id = id;
            var updatedMessage = await _Repository.CanUpdateAsync(data, CurrentAccountId);
            if (!string.IsNullOrWhiteSpace(updatedMessage))
            {
                ModelState.AddModelError("message", updatedMessage);
                return BadRequest(ModelState);
            }
            await _Repository.UpdateAsync(data, CurrentAccountId);
            if (afterUpdated != null)
                return await afterUpdated(data);
            return await Get(data.Id);
        }
        #endregion

        #region _DeleteRequest 处理Delete请求
        /// <summary>
        /// 处理Delete请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="afterDeleted"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _DeleteRequest(string id, Func<Task<IActionResult>> afterDeleted = null)
        {
            var deleteMessage = await _Repository.CanDeleteAsync(id, CurrentAccountId);
            if (!string.IsNullOrWhiteSpace(deleteMessage))
            {
                ModelState.AddModelError("message", deleteMessage);
                return BadRequest(ModelState);
            }
            await _Repository.DeleteAsync(id, CurrentAccountId);
            if (afterDeleted != null)
                return await afterDeleted();
            return Ok();
        }
        #endregion


    }
}