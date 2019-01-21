using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Base.Common.Controllers
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
        protected async Task<IActionResult> _PostRequest(Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> afterCreated)
        {
            var data = await mapping(new T());
            var createdMessage = await _Repository.CanCreateAsync(data, CurrentAccountId);
            if (!string.IsNullOrWhiteSpace(createdMessage))
            {
                ModelState.AddModelError("message", createdMessage);
                return BadRequest(ModelState);
            }
            await _Repository.CreateAsync(data, CurrentAccountId);
            return await afterCreated(data);
        }

        /// <summary>
        /// 重载,注意,该方案传递的data是作为创建动作还是更新动作的关键
        /// 如果data为空,即为创建,但是如果有值且id不为空,即将进行更新操作
        /// data中的其他字段在mapping中匹配
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mapping"></param>
        /// <param name="afterCreated"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PostRequest(T data, Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> afterCreated = null)
        {
            if (data != null && !string.IsNullOrWhiteSpace(data.Id))
            {
                if (afterCreated == null)
                    return await _PutRequest(data.Id, mapping);
                else
                    return await _PutRequest(data.Id, mapping, afterCreated);
            }
            else
            {
                if (afterCreated == null)
                    return await _PostRequest(mapping);
                else
                    return await _PostRequest(mapping, afterCreated);
            }

        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="afterCreated"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PostRequest(Func<T, Task<T>> mapping, Func<T, Task> afterCreated = null)
        {
            var funAfterCreated = new Func<T, Task<IActionResult>>(async (entity) =>
            {
                if (afterCreated != null)
                    await afterCreated(entity);
                return await Get(entity.Id);
            });
            return await _PostRequest(mapping, funAfterCreated);
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
        protected async Task<IActionResult> _PutRequest(string id, Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> afterUpdated)
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
            return await afterUpdated(data);
        }

        protected async Task<IActionResult> _PutRequest(string id, Func<T, Task<T>> mapping, Func<T, Task> afterUpdated = null)
        {
            var funAfterCreated = new Func<T, Task<IActionResult>>(async (entity) =>
            {
                if (afterUpdated != null)
                    await afterUpdated(entity);
                return Ok();
            });
            return await _PutRequest(id, mapping, funAfterCreated);
        }
        #endregion

        #region _DeleteRequest 处理Delete请求
        /// <summary>
        /// 处理Delete请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="afterDeleted"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _DeleteRequest(string id, Func<Task<IActionResult>> afterDeleted)
        {
            var entity = await _Repository.GetByIdAsync(id, CurrentAccountId);
            if (entity == null)
                return NotFound();
            var deleteMessage = await _Repository.CanDeleteAsync(id, CurrentAccountId);
            if (!string.IsNullOrWhiteSpace(deleteMessage))
            {
                ModelState.AddModelError("message", deleteMessage);
                return BadRequest(ModelState);
            }
            await _Repository.DeleteAsync(id, CurrentAccountId);
            return await afterDeleted();
        }

        protected async Task<IActionResult> _DeleteRequest(string id, Func<Task> afterDeleted = null)
        {
            var funAfterDeleted = new Func<Task<IActionResult>>(async () =>
            {
                if (afterDeleted != null)
                    await afterDeleted();
                return Ok();
            });
            return await _DeleteRequest(id, funAfterDeleted);
        }
        #endregion

        #region _BatchDeleteRequest 批量删除数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="afterDeleteLiteral"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _BatchDeleteRequest(string ids, Func<string, Task> afterDeleteLiteral = null)
        {
            var rejectMessages = new List<Dictionary<string, string>>();
            var idArr = ids.Split(",");
            foreach (var id in idArr)
            {
                var deleteMessage = await _Repository.CanDeleteAsync(id, CurrentAccountId);
                if (!string.IsNullOrWhiteSpace(deleteMessage))
                {
                    var dict = new Dictionary<string, string>();
                    dict[id] = deleteMessage;
                    rejectMessages.Add(dict);
                    continue;
                }
                await _Repository.DeleteAsync(id, CurrentAccountId);
                if (afterDeleteLiteral != null)
                    await afterDeleteLiteral(id);
            }
            if (rejectMessages.Count > 0)
                return Ok(rejectMessages);
            return Ok();
        }
        #endregion

    }
}