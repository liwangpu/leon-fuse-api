using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApiServer.Controllers
{
    /// <summary>
    /// 部门管理控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class DepartmentController : ListableController<Department>
    {
        private readonly DepartmentStore _DepartmentStore;

        #region 构造函数
        public DepartmentController(ApiDbContext context)
        : base(new DepartmentStore(context))
        {
            _DepartmentStore = _Store as DepartmentStore;
        }
        #endregion

        #region Get 根据id获取部门信息
        /// <summary>
        /// 根据id获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建部门信息
        /// <summary>
        /// 新建部门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]DepartmentCreateModel model)
        {
            var mapping = new Func<Department, Task<Department>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ParentId = model.ParentId;
                entity.OrganizationId = model.OrganizationId;
                return Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 编辑部门信息
        /// <summary>
        /// 编辑部门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]DepartmentEditModel model)
        {
            var mapping = new Func<Department, Task<Department>>((entity) =>
            {
                entity.Name = model.Name;
                entity.ParentId = model.ParentId;
                entity.Description = model.Description;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region GetByOrgan 根据组织id获取部门信息
        /// <summary>
        /// 根据组织id获取部门信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        [Route("ByOrgan")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DepartmentDTO>), 200)]
        public async Task<IActionResult> GetByOrgan(string organId)
        {
            var dtos = await _DepartmentStore.GetByOrgan(organId);
            return Ok(dtos);
        } 
        #endregion

    }
}