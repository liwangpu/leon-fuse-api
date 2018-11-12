using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region UpdateWorkFlowItem 更新详细工作流程
        /// <summary>
        /// 更新详细工作流程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateWorkFlowItem")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UpdateWorkFlowItem([FromBody]WorkFlowItemEditModel model)
        {
            var mapping = new Func<WorkFlow, Task<WorkFlow>>(async (entity) =>
            {
                var accid = AuthMan.GetAccountId(this);
                var workFlowItems = entity.WorkFlowItems != null ? entity.WorkFlowItems : new List<WorkFlowItem>();

                var destGradeIndex = model.FlowGrade;//目的index
                var originGradeIndex = !string.IsNullOrWhiteSpace(model.Id) ? workFlowItems.First(x => x.Id == model.Id).FlowGrade : 0;//原index

                if (!string.IsNullOrWhiteSpace(model.Id))
                {
                    for (var idx = workFlowItems.Count - 1; idx >= 0; idx--)
                    {
                        var curItem = workFlowItems[idx];

                        if (destGradeIndex < originGradeIndex)
                        {
                            //上移
                            /*
                             * 1)自己本身变成destIndex
                             * 2)原items中,大于等于destIndex且小于自己originIndex都加1
                             */
                            if (curItem.FlowGrade >= destGradeIndex && curItem.FlowGrade < originGradeIndex)
                                curItem.FlowGrade += 1;

                        }
                        else if (destGradeIndex > originGradeIndex)
                        {
                            //下移
                            /*
                             * 1)自己本身变成destIndex
                             * 2)原items中,大于originIndex且小于等于destIndex都减1
                             */
                            if (curItem.FlowGrade > originGradeIndex && curItem.FlowGrade <= destGradeIndex)
                                curItem.FlowGrade -= 1;
                        }
                        else { }

                        //以上提到的自己本身index变为destIndex
                        if (curItem.Id == model.Id)
                        {
                            curItem.Name = model.Name;
                            curItem.Description = model.Description;
                            curItem.OperateRoles = model.OperateRoles;
                            curItem.FlowGrade = destGradeIndex;
                            curItem.SubWorkFlowId = model.SubWorkFlowId;
                            curItem.Modifier = accid;
                            curItem.ModifiedTime = DateTime.Now;
                        }
                    }
                }
                else
                {
                    var newFlowItem = new WorkFlowItem();
                    newFlowItem.Id = GuidGen.NewGUID();
                    newFlowItem.Name = model.Name;
                    newFlowItem.Description = model.Description;
                    newFlowItem.OperateRoles = model.OperateRoles;
                    newFlowItem.FlowGrade = workFlowItems.Count;
                    newFlowItem.SubWorkFlowId = model.SubWorkFlowId;
                    newFlowItem.Creator = accid;
                    newFlowItem.Modifier = accid;
                    newFlowItem.CreatedTime = DateTime.Now;
                    newFlowItem.ModifiedTime = newFlowItem.CreatedTime;
                    newFlowItem.WorkFlow = entity;
                    workFlowItems.Add(newFlowItem);
                }

                entity.WorkFlowItems = workFlowItems;
                entity.Modifier = accid;
                entity.ModifiedTime = DateTime.Now;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.WorkFlowId, mapping);
        }
        #endregion

        #region DeleteWorkFlowItem 删除详细工作流程
        /// <summary>
        /// 删除详细工作流程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteWorkFlowItem")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteWorkFlowItem([FromBody]WorkFlowItemDeleteModel model)
        {
            var mapping = new Func<WorkFlow, Task<WorkFlow>>(async (entity) =>
            {
                var accid = AuthMan.GetAccountId(this);
                var workFlowItems = entity.WorkFlowItems != null ? entity.WorkFlowItems.Where(x => x.Id != model.Id).OrderBy(x => x.FlowGrade).ToList() : new List<WorkFlowItem>();
                for (int idx = 0, len = workFlowItems.Count; idx < len; idx++)
                {
                    var item = workFlowItems[idx];
                    item.FlowGrade = idx;
                }


                entity.WorkFlowItems = workFlowItems;
                entity.Modifier = accid;
                entity.ModifiedTime = DateTime.Now;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.WorkFlowId, mapping);
        } 
        #endregion
    }
}