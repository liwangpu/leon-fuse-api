using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    [Route("/[controller]")]
    public class OrdersController : ListableController<Order, OrderDTO>
    {
        protected IHostingEnvironment _Env;
        #region 构造函数
        public OrdersController(IRepository<Order, OrderDTO> repository, IHostingEnvironment env)
            : base(repository)
        {
            _Env = env;
        }
        #endregion

        #region Get 根据分页查询信息获取订单概要信息
        /// <summary>
        /// 根据分页查询信息获取订单概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrderDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var literal = new Func<Order, IList<Order>, Task<Order>>(async (entity, datas) =>
             {
                 //entity.Content = null;
                 return await Task.FromResult(entity);
             });
            return await _GetPagingRequest(model, null, null, literal);
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
            //return await _GetByIdRequest(id);
            var exist = await _Repository.ExistAsync(id);
            if (!exist)
                return NotFound();
            var dto = await _Repository.GetByIdAsync(id);
            return Ok(dto);
        }
        #endregion

        #region GetOrganOrderFlow 获取组织订单流程信息
        /// <summary> 
        /// 获取组织订单流程信息
        /// </summary>
        /// <returns></returns>
        [Route("GetOrganOrderFlow")]
        [HttpGet]
        [ProducesResponseType(typeof(WorkFlow), 200)]
        public async Task<IActionResult> GetOrganOrderFlow()
        {
            var rootOrgan = await _GetCurrentUserRootOrgan();
            var ruleDetail = await _Repository._DbContext.WorkFlowRuleDetails.Where(x => x.OrganizationId == rootOrgan.Id).FirstOrDefaultAsync();
            if (ruleDetail == null) return Ok();
            return RedirectToAction("Get", "WorkFlow", new { id = ruleDetail.WorkFlowId });
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
            var mapping = new Func<Order, Task<Order>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;

                var details = new List<OrderDetail>();
                if (model.Content != null && model.Content.Count > 0)
                {
                    model.Content.ForEach(item =>
                    {
                        var detail = new OrderDetail();
                        detail.ProductSpecId = item.ProductSpecId;
                        detail.Num = item.Num;
                        detail.UnitPrice = item.UnitPrice;
                        detail.TotalPrice = item.TotalPrice;
                        detail.Remark = item.Remark;
                        details.Add(detail);
                    });
                }
                entity.OrderDetails = details;
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
                var accid = AuthMan.GetAccountId(this);
                entity.Name = model.Name;
                entity.Description = model.Description;
                //entity.Content = model.Content;
                //entity.Icon = model.IconAssetId;
                entity.ModifiedTime = DateTime.Now;
                entity.Modifier = accid;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region AuditOrder 订单审核
        /// <summary>
        /// 订单审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AuditOrder")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> AuditOrder([FromBody]OrderWorkFlowAuditEditModel model)
        {
            var mapping = new Func<Order, Task<Order>>(async (entity) =>
            {
                var accid = AuthMan.GetAccountId(this);

                var logs = await _Repository._DbContext.OrderFlowLogs.Where(x => x.Order == entity).ToListAsync();
                var operateLog = new OrderFlowLog();
                operateLog.Id = GuidGen.NewGUID();
                operateLog.Remark = model.Remark;
                operateLog.CreatedTime = DateTime.Now;
                operateLog.Creator = accid;
                operateLog.Approve = model.Approve;
                operateLog.WorkFlowItemId = model.WorkFlowItemId;
                operateLog.Order = entity;
                _Repository._DbContext.OrderFlowLogs.Add(operateLog);
                await _Repository._DbContext.SaveChangesAsync();
                var workFlowItem = await _Repository._DbContext.WorkFlowItems.Include(x => x.WorkFlow).Where(x => x.Id == model.WorkFlowItemId).FirstOrDefaultAsync();

                if (workFlowItem != null)
                {
                    var workFlow = await _Repository._DbContext.WorkFlows.Include(x => x.WorkFlowItems).Where(x => x == workFlowItem.WorkFlow).FirstOrDefaultAsync();

                    if (model.Approve)
                    {
                        var nextWorkFlowItem = workFlow.WorkFlowItems.Where(x => x.FlowGrade == workFlowItem.FlowGrade + 1).FirstOrDefault();
                        if (nextWorkFlowItem != null)
                            entity.WorkFlowItemId = nextWorkFlowItem.Id;
                    }
                    else
                    {
                        var lastWorkFlowItem = workFlow.WorkFlowItems.Where(x => x.FlowGrade == workFlowItem.FlowGrade - 1).FirstOrDefault();
                        if (lastWorkFlowItem != null)
                            entity.WorkFlowItemId = lastWorkFlowItem.Id;
                    }
                }

                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.OrderId, mapping);
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
        public async Task<IActionResult> UpdateCustomerMessage([FromBody]OrderCustomerEditModel model)
        {
            var mapping = new Func<Order, Task<Order>>(async (entity) =>
            {
                var accid = AuthMan.GetAccountId(this);
                entity.CustomerName = model.CustomerName;
                entity.CustomerPhone = model.CustomerPhone;
                entity.CustomerAddress = model.CustomerAddress;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.OrderId, mapping);
        }
        #endregion

        #region UpdateOrderDetail 更新订单详情信息
        /// <summary>
        /// 更新订单详情信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateOrderDetail")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UpdateOrderDetail([FromBody]OrderDetailEditModel model)
        {
            var mapping = new Func<Order, Task<Order>>(async (entity) =>
            {
                var accid = AuthMan.GetAccountId(this);
                var detail = await _Repository._DbContext.OrderDetails.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (detail != null)
                {
                    detail.Num = model.Num;
                    detail.TotalPrice = model.TotalPrice;
                    detail.Remark = model.Remark;
                    if (!string.IsNullOrWhiteSpace(model.AttachIds))
                    {
                        var idArr = model.AttachIds.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);
                        detail.AttachmentIds = string.Join(",", idArr);
                    }
                    else
                        detail.AttachmentIds = string.Empty;
                    _Repository._DbContext.OrderDetails.Update(detail);
                    await _Repository._DbContext.SaveChangesAsync();
                }
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.OrderId, mapping);
        }
        #endregion

        [AllowAnonymous]
        [HttpGet("ExportOrder/{orderId}", Name = "ExportOrder")]
        public async Task<IActionResult> ExportOrder(string orderId)
        {
            var order = await _Repository.GetByIdAsync(orderId);
            if (order == null)
                return NotFound();

            var buffer = new byte[0];
            using (var package = new ExcelPackage())
            {
                var workbox = package.Workbook;
                var sheet1 = workbox.Worksheets.Add("订单详情");
                var gapRow = 1;
                var gapCol = 1;
                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 1, gapRow + 5, gapCol + 2])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.WrapText = true;
                    rng.Merge = true;

                    var logoImgPath = Path.Combine(_Env.WebRootPath, "Attachments", "ehome-logo.jpg");
                    if (System.IO.File.Exists(logoImgPath))
                    {
                        var logoImg = Image.FromFile(logoImgPath);
                        var picture = sheet1.Drawings.AddPicture("ehome-logo", logoImg);
                        picture.SetPosition(gapRow, 1, gapRow, 1);
                        picture.SetSize(166, 114);
                    }
                }

                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 3, gapRow + 3, gapCol + 7])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.WrapText = true;
                    rng.Merge = true;
                    rng.Value = "E-Home家装设计平台销售订单";
                    rng.Style.Font.Bold = true;
                    var colFromHex = ColorTranslator.FromHtml("#D0CECE");
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                using (var rng = sheet1.Cells[gapRow + 4, gapCol + 3, gapRow + 5, gapCol + 7])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.WrapText = true;
                    rng.Merge = true;
                    rng.Style.Font.Bold = true;
                    rng.Value = order.Name;
                }


                sheet1.Cells[gapRow + 1, gapCol + 8].Value = "订单编号";
                sheet1.Cells[gapRow + 2, gapCol + 8].Value = "收货地址";
                sheet1.Cells[gapRow + 3, gapCol + 8].Value = "产品数量";
                sheet1.Cells[gapRow + 4, gapCol + 8].Value = "第一次验货";
                sheet1.Cells[gapRow + 5, gapCol + 8].Value = "第二次验货";

                sheet1.Cells[gapRow + 1, gapCol + 8].Value = "订单编号";

                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 9, gapRow + 1, gapCol + 10])
                {
                    rng.Style.WrapText = true;
                    rng.Merge = true;
                    rng.Value = order.OrderNo;
                }
                using (var rng = sheet1.Cells[gapRow + 2, gapCol + 9, gapRow + 2, gapCol + 13])
                {
                    rng.Style.WrapText = true;
                    rng.Merge = true;
                    rng.Value = order.CustomerAddress;
                }

                sheet1.Cells[gapRow + 3, gapCol + 9].Value = order.TotalNum;

                sheet1.Cells[gapRow + 1, gapCol + 11].Value = "收货人";
                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 12, gapRow + 1, gapCol + 13])
                {
                    rng.Style.WrapText = true;
                    rng.Merge = true;
                    rng.Value = order.CustomerName;
                }

                sheet1.Cells[gapRow + 3, gapCol + 10].Value = "订单金额";
                sheet1.Cells[gapRow + 4, gapCol + 10].Value = "首付款";
                sheet1.Cells[gapRow + 5, gapCol + 10].Value = "尾款";
                sheet1.Cells[gapRow + 4, gapCol + 11].Value = order.TotalPrice / 2;
                sheet1.Cells[gapRow + 5, gapCol + 11].Value = order.TotalPrice / 2;

                sheet1.Cells[gapRow + 3, gapCol + 11].Value = order.TotalPrice;
                sheet1.Cells[gapRow + 3, gapCol + 12].Value = "发货日期";
                sheet1.Cells[gapRow + 4, gapCol + 12].Value = "专属客服";
                sheet1.Cells[gapRow + 4, gapCol + 13].Value = order.CreatorName;
                sheet1.Cells[gapRow + 5, gapCol + 12].Value = "联系方式";
                sheet1.Cells[gapRow + 5, gapCol + 13].Value = order.CustomerPhone;

                sheet1.Cells[gapRow + 6, gapCol + 1].Value = "序号";
                sheet1.Cells[gapRow + 6, gapCol + 2].Value = "区域";
                sheet1.Cells[gapRow + 6, gapCol + 3].Value = "图片";
                sheet1.Cells[gapRow + 6, gapCol + 4].Value = "产品名称";
                sheet1.Cells[gapRow + 6, gapCol + 5].Value = "产品类型";
                sheet1.Cells[gapRow + 6, gapCol + 6].Value = "产品描述";
                sheet1.Cells[gapRow + 6, gapCol + 7].Value = "单位";
                sheet1.Cells[gapRow + 6, gapCol + 8].Value = "数量";
                sheet1.Cells[gapRow + 6, gapCol + 9].Value = "单价";
                sheet1.Cells[gapRow + 6, gapCol + 10].Value = "小计";
                sheet1.Cells[gapRow + 6, gapCol + 11].Value = "材质";
                sheet1.Cells[gapRow + 6, gapCol + 12].Value = "面料";
                sheet1.Cells[gapRow + 6, gapCol + 13].Value = "备注";

                for (int idx = 0, len = order.OrderDetails.Count; idx < len; idx++)
                {
                    var ditem = order.OrderDetails[idx];
                    sheet1.Cells[gapRow + idx + 7, gapCol + 1].Value = idx + 1;
                    if (!string.IsNullOrWhiteSpace(ditem.Icon))
                    {
                        ditem.Icon = ditem.Icon.Replace("/upload/", "");
                        var iconImgPath = Path.Combine(_Env.WebRootPath, "upload", ditem.Icon);
                        if (System.IO.File.Exists(iconImgPath))
                        {
                            var logoImg = Image.FromFile(iconImgPath);
                            var picture = sheet1.Drawings.AddPicture("icon" + idx, logoImg);
                            picture.SetPosition(gapRow + 6 + idx, 1, gapCol + 2, 1);
                            picture.SetSize(80, 80);
                        }
                    }

                    sheet1.Cells[gapRow + idx + 7, gapCol + 4].Value = ditem.ProductName;
                    sheet1.Cells[gapRow + idx + 7, gapCol + 5].Value = ditem.ProductCategoryName;
                    sheet1.Cells[gapRow + idx + 7, gapCol + 6].Value = ditem.ProductDescription;
                    sheet1.Cells[gapRow + idx + 7, gapCol + 7].Value = ditem.ProductUnit;
                    sheet1.Cells[gapRow + idx + 7, gapCol + 8].Value = ditem.Num;
                    sheet1.Cells[gapRow + idx + 7, gapCol + 9].Value = ditem.UnitPrice;
                    sheet1.Cells[gapRow + idx + 7, gapCol + 10].Value = ditem.TotalPrice;

                    sheet1.Row(gapRow + idx + 7).Height = 63;

                }
                sheet1.Cells[gapRow + order.OrderDetails.Count + 7, gapCol + 7].Value = "数量合计";
                sheet1.Cells[gapRow + order.OrderDetails.Count + 7, gapCol + 8].Value = order.TotalNum;
                sheet1.Cells[gapRow + order.OrderDetails.Count + 7, gapCol + 9].Value = "总价合计";
                sheet1.Cells[gapRow + order.OrderDetails.Count + 7, gapCol + 10].Value = order.TotalPrice;

                //标题栏居中对齐,统一设置背景色
                using (var rng = sheet1.Cells[gapRow + 6, gapCol + 1, gapRow + 6, gapCol + 13])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Font.Bold = true;
                    var colFromHex = ColorTranslator.FromHtml("#D0CECE");
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 8, gapRow + 5, gapCol + 8])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Font.Bold = true;
                    var colFromHex = ColorTranslator.FromHtml("#D0CECE");
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                using (var rng = sheet1.Cells[gapRow + 3, gapCol + 10, gapRow + 5, gapCol + 10])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Font.Bold = true;
                    var colFromHex = ColorTranslator.FromHtml("#D0CECE");
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 11, gapRow + 1, gapCol + 11])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Font.Bold = true;
                    var colFromHex = ColorTranslator.FromHtml("#D0CECE");
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                using (var rng = sheet1.Cells[gapRow + 3, gapCol + 12, gapRow + 5, gapCol + 12])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Font.Bold = true;
                    var colFromHex = ColorTranslator.FromHtml("#D0CECE");
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }


                using (var rng = sheet1.Cells[gapRow + 1, gapCol + 1, gapRow + 7 + order.OrderDetails.Count, gapCol + 13])
                {
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                sheet1.Row(gapRow + 61).Height = 18;
                sheet1.Row(gapRow + 2).Height = 18;
                sheet1.Row(gapRow + 3).Height = 18;
                sheet1.Row(gapRow + 4).Height = 18;
                sheet1.Row(gapRow + 5).Height = 18;
                sheet1.Row(gapRow + 6).Height = 25;
                sheet1.Row(gapRow + 7 + order.OrderDetails.Count).Height = 18;

                sheet1.Column(2).Width = 12;
                sheet1.Column(3).Width = 12;
                sheet1.Column(4).Width = 12;
                sheet1.Column(5).Width = 12;
                sheet1.Column(6).Width = 12;
                sheet1.Column(7).Width = 12;
                sheet1.Column(8).Width = 12;
                sheet1.Column(9).Width = 12;
                sheet1.Column(10).Width = 12;
                sheet1.Column(11).Width = 12;
                sheet1.Column(12).Width = 12;
                sheet1.Column(13).Width = 12;
                sheet1.Column(14).Width = 18;
                buffer = package.GetAsByteArray();
            }


            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "report.xlsx");
        }

    }
}
