using Apps.Base.Common.Attributes;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 部门控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ListviewController<Department>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public DepartmentController(IRepository<Department> repository, AppDbContext context)
            : base(repository)
        {
            _Context = context;
        }

        #endregion

        #region Get 根据分页获取部门信息
        /// <summary>
        /// 根据分页获取部门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<DepartmentDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var toDTO = new Func<Department, Task<DepartmentDTO>>(async (entity) =>
            {
                var dto = new DepartmentDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        #region Get 根据Id获取部门信息
        /// <summary>
        /// 根据Id获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<Department, Task<DepartmentDTO>>(async (entity) =>
            {
                var dto = new DepartmentDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 创建部门
        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        public async Task<IActionResult> Post([FromBody]DepartmentCreateModel model)
        {
            var mapping = new Func<Department, Task<Department>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更改部门信息
        /// <summary>
        /// 更改部门信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        public async Task<IActionResult> Put([FromBody]DepartmentUpdateModel model)
        {
            var mapping = new Func<Department, Task<Department>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion
    }
}