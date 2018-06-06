using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Controllers.Asset
{
    [Authorize]
    [Route("/[controller]")]
    public class MediaShareController : CommonController<MediaShareResource, MediaShareResourceDTO>
    {
        #region 构造函数
        public MediaShareController(ApiDbContext context)
        : base(new MediaShareStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取媒体文件概要信息
        /// <summary>
        /// 根据分页查询信息获取媒体文件概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MediaShareResourceDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var qMapping = new Action<List<string>>((query) =>
            {

            });
            return await _GetPagingRequest(model, qMapping);
        }
        #endregion

        #region Get 根据id获取媒体文件信息
        /// <summary>
        /// 根据id获取媒体文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MediaShareResourceDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();
            var dto = await _Store.GetByIdAsync(id);
            var curUtcTime = DateTime.UtcNow;
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (curUtcTime >= dtDateTime.AddSeconds(dto.StartShareTimeStamp).ToLocalTime())
            {
                if (dto.StopShareTimeStamp > 0)
                {
                    if (curUtcTime > dtDateTime.AddSeconds(dto.StopShareTimeStamp).ToLocalTime())
                        return Forbid();
                }
            }
            return Ok(dto);
        }
        #endregion

        #region Post 新建媒体文件信息
        /// <summary>
        /// 新建媒体文件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MediaShareResourceDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MediaShareResourceCreateModel model)
        {
            var mapping = new Func<MediaShareResource, Task<MediaShareResource>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.MediaId = model.MediaId;
                entity.StartShareTimeStamp = model.StartShareTimeStamp;
                entity.StopShareTimeStamp = model.StopShareTimeStamp;
                entity.Password = model.Password;
                entity.ResourceType = (int)ResourceTypeEnum.NoLimit;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新媒体文件信息
        /// <summary>
        /// 更新媒体文件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MediaShareResourceDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MediaShareResourceEditModel model)
        {
            var mapping = new Func<MediaShareResource, Task<MediaShareResource>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.StartShareTimeStamp = model.StartShareTimeStamp;
                entity.StopShareTimeStamp = model.StopShareTimeStamp;
                entity.Password = model.Password;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion


    }
}