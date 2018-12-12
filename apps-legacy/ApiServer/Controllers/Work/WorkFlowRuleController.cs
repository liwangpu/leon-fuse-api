using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using BambooCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Controllers.Work
{
    [Route("/[controller]")]
    public class WorkFlowRuleController : ListableController<WorkFlowRule, WorkFlowRuleDTO>
    {

        #region 构造函数
        public WorkFlowRuleController(IRepository<WorkFlowRule, WorkFlowRuleDTO> repository) : base(repository)
        {
        }
        #endregion

        #region Get 根据分页查询信息获取工作流规则概要信息
        /// <summary>
        /// 根据分页查询信息获取工作流规则概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<WorkFlowRuleDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var qMapping = new Action<List<string>>((query) =>
            {

            });
            return await _GetPagingRequest(model, qMapping);
        }
        #endregion

        #region Get 根据id获取工作流规则信息
        /// <summary>
        /// 根据id获取工作流规则信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkFlowRuleDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {

            var handle = new Func<WorkFlowRuleDTO, Task<IActionResult>>(async (dto) =>
           {
               var organId = await _GetCurrentUserOrganId();
               var define = await _Repository._DbContext.WorkFlowRuleDetails.Where(x => x.KeyWord == dto.Keyword && x.OrganizationId == organId).FirstOrDefaultAsync();
               if (define != null)
               {
                   var workflow = await _Repository._DbContext.WorkFlows.Where(x => x.Id == define.WorkFlowId).FirstOrDefaultAsync();
                   if (workflow != null)
                       dto.WorkFlowName = workflow.Name;
               }

               return Ok(dto);
           });
            return await _GetByIdRequest(id, handle);
        }
        #endregion

        #region Post 新建工作流规则信息
        /// <summary>
        /// 新建工作流规则信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]WorkFlowRuleCreateModel model)
        {
            var mapping = new Func<WorkFlowRule, Task<WorkFlowRule>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Keyword = model.Keyword;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新工作流规则信息
        /// <summary>
        /// 更新工作流规则信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]WorkFlowRuleEditModel model)
        {
            var mapping = new Func<WorkFlowRule, Task<WorkFlowRule>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Keyword = model.Keyword;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region DefineRuleDetail 指定工作流规则的工作流
        /// <summary>
        /// 指定工作流规则的工作流
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DefineRuleDetail")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(WorkFlowRuleDetail), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DefineRuleDetail([FromBody]WorkFlowRuleDefineModel model)
        {
            var organId = await _GetCurrentUserOrganId();
            var define = await _Repository._DbContext.WorkFlowRuleDetails.Where(x => x.KeyWord == model.Keyword && x.OrganizationId == organId).FirstOrDefaultAsync();
            if (define == null)
            {
                define = new WorkFlowRuleDetail();
                define.Id = GuidGen.NewGUID();
                define.KeyWord = model.Keyword;
                define.OrganizationId = organId;
                define.WorkFlowId = model.WorkFlowId;
                _Repository._DbContext.WorkFlowRuleDetails.Add(define);
            }
            else
            {
                define.WorkFlowId = model.WorkFlowId;
                _Repository._DbContext.WorkFlowRuleDetails.Update(define);
            }

            await _Repository._DbContext.SaveChangesAsync();

            return Ok(define);
        }
        #endregion
    }
}