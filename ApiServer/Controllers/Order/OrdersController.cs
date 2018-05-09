using ApiModel.Entities;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
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
    public class OrdersController : Controller
    {
        private readonly Repository<Order> repo;
        private readonly OrderStore _OrderStore;

        public OrdersController(Data.ApiDbContext context)
        {
            repo = new Repository<Order>(context);
            _OrderStore = new OrderStore(context);
        }

        #region Get 根据分页查询信息获取订单概要信息
        /// <summary>
        /// 根据分页查询信息获取订单概要信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrderDTO>), 200)]
        [ProducesResponseType(typeof(PagedData<OrderDTO>), 400)]
        public async Task<PagedData<OrderDTO>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            var accid = AuthMan.GetAccountId(this);
            if (string.IsNullOrEmpty(search))
                return await _OrderStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc);
            else
                return await _OrderStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }
        #endregion

        #region Get 根据id获取订单信息
        /// <summary>
        /// 根据id获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _OrderStore.Exist(id);
            if (!exist)
                return NotFound();
            var canRead = await _OrderStore.CanRead(accid, id);
            if (!canRead)
                return Forbid();
            var dto = await _OrderStore.GetByIdAsync(accid, id);
            return Ok(dto);
        }
        #endregion

        #region Post 创建订单
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]OrderCreateModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var order = new Order();
            order.Name = value.Name;
            order.Description = value.Description;
            await _OrderStore.CanCreate(accid, order, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);

            var dto = await _OrderStore.CreateAsync(accid, order);
            return Ok(dto);
        }
        #endregion

        #region Put 更新订单信息
        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]OrderEditModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _OrderStore.Exist(value.Id);
            if (!exist)
                return NotFound();

            var order = await _OrderStore._GetByIdAsync(value.Id);
            order.Name = value.Name;
            order.Description = value.Description;

            await _OrderStore.CanUpdate(accid, order, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            var dto = await _OrderStore.UpdateAsync(accid, order);
            return Ok(dto);
        }
        #endregion

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool bOk = await repo.DeleteAsync(AuthMan.GetAccountId(this), id);
            if (bOk)
            {
                return Ok();
            }
            return NotFound(); // 或者权限不够
        }

        //[Route("ChangeState")]
        //[HttpPost]
        //public async Task<IActionResult> ChangeState(string id, [FromBody]OrderStateItem state)
        //{
        //    if (ModelState.IsValid == false)
        //        return BadRequest(ModelState);

        //    string accid = AuthMan.GetAccountId(this);
        //    var ok = await repo.CanUpdateAsync(accid, id);
        //    if (ok == false)
        //        return Forbid();

        //    var res = await repo.GetAsync(accid, id);
        //    if (res == null)
        //        return NotFound();

        //    res.StateTime = DateTime.UtcNow;
        //    if (res.OrderStates == null)
        //    {
        //        res.OrderStates = new List<OrderStateItem>();
        //    }
        //    state.Id = GuidGen.NewGUID();
        //    state.Order = res;
        //    state.OrderId = res.Id;
        //    state.OldState = res.State;
        //    state.OperateTime = DateTime.UtcNow;
        //    res.State = state.NewState;
        //    res.OrderStates.Add(state);
        //    await repo.SaveChangesAsync();
        //    return Ok();
        //}

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
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var exist = await _OrderStore.Exist(id);
            if (!exist)
                return NotFound();

            string accid = AuthMan.GetAccountId(this);
            var order = await _OrderStore._GetByIdAsync(id);
            order.Content = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            await _OrderStore.CanUpdate(accid, order, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            var dto = await _OrderStore.UpdateAsync(accid, order);
            return Ok(dto);
        } 
        #endregion

        #region NewOne Post,Put示例数据
        /// <summary>
        /// Post,Put示例数据
        /// </summary>
        /// <returns></returns>
        [Route("NewOne")]
        [HttpGet]
        [ProducesResponseType(typeof(OrderCreateModel), 200)]
        public IActionResult NewOne()
        {
            return Ok(new OrderCreateModel());
        }
        #endregion
    }
}
