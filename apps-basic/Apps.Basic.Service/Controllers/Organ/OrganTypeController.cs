using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Contexts;
using Apps.Basic.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 组织类型类型控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OrganTypeController : ServiceBaseController<OrganizationType>
    {

        protected readonly AppDbContext _Context;

        #region 构造函数
        public OrganTypeController(IRepository<OrganizationType> repository, AppDbContext context)
            : base(repository)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取组织类型信息
        /// <summary>
        /// 根据分页获取组织类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrganizationTypeDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var toDTO = new Func<OrganizationType, Task<OrganizationTypeDTO>>(async (entity) =>
            {
                var dto = new OrganizationTypeDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.IsInner = entity.IsInner;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        #region Get 根据Id获取组织类型信息
        /// <summary>
        /// 根据Id获取组织类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrganizationTypeDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<OrganizationType, Task<OrganizationTypeDTO>>(async (entity) =>
            {
                var dto = new OrganizationTypeDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.IsInner = entity.IsInner;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

    }
}