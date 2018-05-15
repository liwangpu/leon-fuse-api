﻿using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class OrdersController : ListableController<Order, OrderDTO>
    {
        #region 构造函数
        public OrdersController(ApiDbContext context)
        : base(new OrderStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取订单概要信息
        /// <summary>
        /// 根据分页查询信息获取订单概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrderDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model);
        }
        #endregion

        #region Get 根据id获取订单信息
        /// <summary>
        /// 根据id获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 创建订单
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]OrderCreateModel model)
        {
            var mapping = new Func<Order, Task<Order>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Content = model.Content;
                return Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新订单信息
        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]OrderEditModel model)
        {
            var mapping = new Func<Order, Task<Order>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Content = model.Content;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region ChangeState 修改订单状态
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [Route("ChangeState")]
        [HttpPost]
        public async Task<IActionResult> ChangeState(string id, [FromBody]OrderStateItem state)
        {
            var mapping = new Func<Order, Task<Order>>((entity) =>
            {
                entity.StateTime = DateTime.UtcNow;
                if (entity.OrderStates == null)
                {
                    entity.OrderStates = new List<OrderStateItem>();
                }
                state.Id = GuidGen.NewGUID();
                state.Order = entity;
                state.OrderId = entity.Id;
                state.OldState = entity.State;
                state.OperateTime = DateTime.UtcNow;
                entity.State = state.NewState;
                entity.OrderStates.Add(state);
                return Task.FromResult(entity);
            });
            return await _PutRequest(id, mapping);
        }
        #endregion

        #region ChangeContent 更新订单详情信息
        /// <summary>
        /// 更新订单详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Route("ChangeContent")]
        [HttpPost]
        public async Task<IActionResult> ChangeContent(string id, [FromBody]OrderContent content)
        {
            var mapping = new Func<Order, Task<Order>>((entity) =>
            {
                entity.Content = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                return Task.FromResult(entity);
            });
            return await _PutRequest(id, mapping);
        }
        #endregion

    }
}
