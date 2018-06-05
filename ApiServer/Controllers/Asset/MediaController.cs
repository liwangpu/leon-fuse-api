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

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class MediaController : ListableController<Media, MediaDTO>
    {
        #region 构造函数
        public MediaController(ApiDbContext context)
        : base(new MediaStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取媒体文件概要信息
        /// <summary>
        /// 根据分页查询信息获取媒体文件概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MediaDTO>), 200)]
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
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MediaDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
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
        [ProducesResponseType(typeof(MediaDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MediaCreateModel model)
        {
            var mapping = new Func<Media, Task<Media>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.FileAssetId = model.FileAssetId;
                entity.Rotation = model.Rotation;
                entity.Location = model.Location;
                entity.Type = model.Type;
                entity.SolutionId = model.SolutionId;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
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
        [ProducesResponseType(typeof(MediaDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MediaEditModel model)
        {
            var mapping = new Func<Media, Task<Media>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.FileAssetId = model.FileAssetId;
                entity.Rotation = model.Rotation;
                entity.Location = model.Location;
                entity.Type = model.Type;
                entity.SolutionId = model.SolutionId;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion


        //[HttpPost]
        //[ValidateModel]
        //[ProducesResponseType(typeof(MediaDTO), 200)]
        //[ProducesResponseType(typeof(ValidationResultModel), 400)]
        //public async Task<IActionResult> CreateSharedMedia([FromBody]MediaShareResourceCreateModel model)
        //{
        //    var mapping = new Func<Media, Task<Media>>(async (entity) =>
        //    {
        //        var resources = entity.MediaShareResources != null ? entity.MediaShareResources : new List<MediaShareResource>();
        //        var res = new MediaShareResource();
        //        res.MediaId = model.MediaId;
        //        res.Media = entity;
        //        res.StartShareTimeStamp = model.StartShareTimeStamp;
        //        res.StopShareTimeStamp = model.StopShareTimeStamp;
        //        res.Password = model.Password;
        //        return await Task.FromResult(entity);
        //    });
        //    return await _PutRequest(model.MediaId, mapping);
        //}
    }
}