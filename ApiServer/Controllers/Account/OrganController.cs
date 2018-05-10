﻿using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 组织管理控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class OrganController : ListableController<Organization>
    {
        private readonly OrganizationStore _OrganStore;

        #region 构造函数
        public OrganController(ApiDbContext context)
        : base(new OrganizationStore(context))
        {
            _OrganStore = _Store as OrganizationStore;
        }
        #endregion

        #region Get 根据分页查询信息获取组织概要信息
        /// <summary>
        /// 根据分页查询信息获取组织概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrganizationDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model);
        }
        #endregion

        #region Get 根据id获取组织信息
        /// <summary>
        /// 根据id获取组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建组织信息
        /// <summary>
        /// 新建组织信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(OrganizationDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]OrganizationCreateModel model)
        {
            var mapping = new Func<Organization, Task<Organization>>((entity) =>
            {
                entity.Name = model.Name;
                entity.ParentId = model.ParentId;
                entity.Description = model.Description;
                entity.Mail = !string.IsNullOrWhiteSpace(model.Mail) ? model.Mail.Trim() : string.Empty;
                entity.Location = model.Location;
                return Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 修改组织信息
        /// <summary>
        /// 修改组织信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrganizationDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]OrganizationEditModel model)
        {
            var mapping = new Func<Organization, Task<Organization>>((entity) =>
            {
                entity.Name = model.Name;
                entity.ParentId = model.ParentId;
                entity.Description = model.Description;
                entity.Mail = !string.IsNullOrWhiteSpace(model.Mail) ? model.Mail.Trim() : string.Empty;
                entity.Location = model.Location;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region GetOwner 获取组织管理员信息
        /// <summary>
        /// 获取组织管理员信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Owner")]
        [ProducesResponseType(typeof(AccountDTO), 200)]
        public async Task<IActionResult> GetOwner(string organId)
        {
            var dto = await _OrganStore.GetOrganOwner(organId);
            return Ok(dto);
        }
        #endregion

    }
}