using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Export.Services;
using Apps.MoreJee.Export.Services;
using Apps.OMS.Data.Entities;
using Apps.OMS.Export.DTOs;
using Apps.OMS.Export.Models;
using Apps.OMS.Export.Services;
using Apps.OMS.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Apps.OMS.Service.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class OrderController : ListviewController<Order>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public OrderController(IRepository<Order> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取订单信息
        /// <summary>
        /// 根据分页获取订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrderDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);

            var toDTO = new Func<Order, Task<OrderDTO>>(async (entity) =>
            {
                var dto = new OrderDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.TotalNum = entity.TotalNum;
                dto.TotalPrice = entity.TotalPrice;
                dto.OrderNo = entity.OrderNo;

                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        #region Get 根据Id获取订单信息
        /// <summary>
        /// 根据Id获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);
            var productSpecMicroService = new ProductSpecMicroService(_AppConfig.APIGatewayServer, Token);
            var productMicroService = new ProductMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<Order, Task<OrderDTO>>(async (entity) =>
            {
                var dto = new OrderDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.TotalNum = entity.TotalNum;
                dto.TotalPrice = entity.TotalPrice;
                dto.OrderNo = entity.OrderNo;

                #region OrderDetails
                var details = new List<OrderDetailDTO>();
                if (entity.OrderDetails != null && entity.OrderDetails.Count > 0)
                {
                    foreach (var it in entity.OrderDetails)
                    {
                        var ditem = new OrderDetailDTO();
                        ditem.Id = it.Id;
                        ditem.Remark = it.Remark;
                        ditem.Num = it.Num;
                        ditem.UnitPrice = it.UnitPrice;
                        ditem.TotalPrice = Math.Round(it.UnitPrice * it.Num, 2, MidpointRounding.AwayFromZero);
                        ditem.AttachmentIds = it.AttachmentIds;

                        await productSpecMicroService.GetById(it.ProductSpecId, (spec) =>
                          {
                              ditem.ProductSpecId = spec.Id;
                              ditem.ProductSpecName = spec.Name;
                              ditem.ProductId = spec.ProductId;
                              ditem.Icon = spec.Icon;
                          });

                        await productMicroService.GetById(ditem.ProductId, (prod) =>
                         {
                             ditem.ProductName = prod.Name;
                             ditem.ProductCategoryId = prod.CategoryId;
                             ditem.ProductCategoryName = prod.CategoryName;
                             ditem.ProductUnit = prod.Unit;
                             ditem.ProductDescription = prod.Description;
                         });

                        details.Add(ditem);
                    }
                }
                dto.OrderDetails = details;
                #endregion

                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 创建订单
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]OrderCreateModel model)
        {
            var mapping = new Func<Order, Task<Order>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.OrganizationId = CurrentAccountOrganizationId;

                var details = new List<OrderDetail>();
                if (model.Content != null && model.Content.Count > 0)
                {
                    model.Content.ForEach(item =>
                    {
                        var detail = new OrderDetail();
                        detail.Id = GuidGen.NewGUID();
                        detail.ProductSpecId = item.ProductSpecId;
                        detail.Num = item.Num;
                        detail.UnitPrice = item.UnitPrice;
                        detail.Remark = item.Remark;
                        detail.CreatedTime = DateTime.Now;
                        detail.ModifiedTime = detail.CreatedTime;
                        detail.Creator = CurrentAccountId;
                        detail.Modifier = CurrentAccountId;
                        detail.OrganizationId = CurrentAccountOrganizationId;
                        details.Add(detail);
                    });
                }
                entity.OrderDetails = details;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 编辑订单信息
        /// <summary>
        /// 编辑订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody] OrderUpdateModel model)
        {
            var mapping = new Func<Order, Task<Order>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;

                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region UpdateCustomerMessage 更新订单用户信息
        /// <summary>
        /// 更新订单用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateCustomerMessage")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UpdateCustomerMessage([FromBody]OrderCustomerUpdateModel model)
        {
            var mapping = new Func<Order, Task<Order>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;

                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.OrderId, mapping);
        }
        #endregion
    }
}