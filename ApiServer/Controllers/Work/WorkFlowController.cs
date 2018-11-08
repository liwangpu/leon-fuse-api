using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("/[controller]")]
    public class WorkFlowController : ListableController<WorkFlow, WorkFlowDTO>
    {
        #region 构造函数
        public WorkFlowController(IRepository<WorkFlow, WorkFlowDTO> repository)
        : base(repository)
        {
        }
        #endregion

        #region Get 根据分页查询信息获取工作流程概要信息
        /// <summary>
        /// 根据分页查询信息获取工作流程概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<WorkFlowDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var qMapping = new Action<List<string>>((query) =>
            {

            });
            return await _GetPagingRequest(model, qMapping);
        }
        #endregion

        #region Get 根据id获取工作流程信息
        /// <summary>
        /// 根据id获取工作流程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建工作流程信息
        /// <summary>
        /// 新建工作流程信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]WorkFlowCreateModel model)
        {
            var mapping = new Func<WorkFlow, Task<WorkFlow>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.ApplyOrgans = model.ApplyOrgans;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新工作流程信息
        /// <summary>
        /// 更新工作流程信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]WorkFlowEditModel model)
        {
            var mapping = new Func<WorkFlow, Task<WorkFlow>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.ApplyOrgans = model.ApplyOrgans;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion


        [Route("CreateWorkFlowItem")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> CreateWorkFlowItem([FromBody]WorkFlowItemEditModel model)
        {

            var mapping = new Func<WorkFlow, Task<WorkFlow>>(async (entity) =>
            {
                var accid = AuthMan.GetAccountId(this);
                var workFlowItems = entity.WorkFlowItems != null ? entity.WorkFlowItems : new List<WorkFlowItem>();
                var exit = false;
                for (var idx = workFlowItems.Count - 1; idx >= 0; idx--)
                {
                    var curItem = workFlowItems[idx];
                    if (curItem.Id == model.Id)
                    {
                        curItem.Name = model.Name;
                        curItem.Description = model.Description;
                        curItem.OperateRoles = model.OperateRoles;
                        curItem.FlowGrade = workFlowItems.Count;
                        curItem.SubWorkFlowId = model.SubWorkFlowId;
                        curItem.Modifier = accid;
                        curItem.ModifiedTime = DateTime.Now;
                        exit = true;
                        break;
                    }
                }




                //var newFlowItem = new WorkFlowItem();
                //newFlowItem.Name = model.Name;
                //newFlowItem.Description = model.Description;
                //newFlowItem.OperateRoles = model.OperateRoles;
                //newFlowItem.FlowGrade = workFlowItems.Count;
                //newFlowItem.SubWorkFlowId = model.SubWorkFlowId;
                //newFlowItem.Creator = accid;
                //newFlowItem.Modifier = accid;
                //newFlowItem.CreatedTime = DateTime.Now;
                //newFlowItem.ModifiedTime = newFlowItem.CreatedTime;
                //newFlowItem.WorkFlow = entity;

                //entity.Modifier = accid;
                //entity.ModifiedTime = newFlowItem.CreatedTime;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.ParentFlowId, mapping);
        }
    }
}