﻿using Apps.Base.Common;
using Apps.Base.Common.Controllers;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Export.Services;
using Apps.OMS.Data.Entities;
using Apps.OMS.Export.DTOs;
using Apps.OMS.Export.Models;
using Apps.OMS.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.OMS.Service.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MemberController : ListviewController<Member>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public MemberController(IRepository<Member> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取会员信息
        /// <summary>
        /// 根据分页获取会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MemberDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);

            var toDTO = new Func<Member, Task<MemberDTO>>(async (entity) =>
            {
                var dto = new MemberDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.Province = entity.Province;
                dto.City = entity.City;
                dto.Area = entity.Area;
                dto.Company = entity.Company;

                var account = await accountMicroService.GetById(entity.AccountId);
                if (account != null)
                {
                    dto.Name = account.Name;
                    dto.Description = account.Description;
                    dto.Phone = account.Phone;
                    dto.Mail = account.Mail;
                    dto.Icon = account.Icon;
                }

                dto.Inviter = entity.Inviter;
                dto.InviterName = await accountMicroService.GetNameById(entity.Inviter);

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

        #region Get 根据Id获取会员信息
        /// <summary>
        /// 根据Id获取会员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MemberDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);

            var toDTO = new Func<Member, Task<MemberDTO>>(async (entity) =>
            {
                var dto = new MemberDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.Province = entity.Province;
                dto.City = entity.City;
                dto.Area = entity.Area;
                dto.Company = entity.Company;

                var account = await accountMicroService.GetById(entity.AccountId);
                if (account != null)
                {
                    dto.Name = account.Name;
                    dto.Description = account.Description;
                    dto.Phone = account.Phone;
                    dto.Mail = account.Mail;
                    dto.Icon = account.Icon;
                }

                dto.Inviter = entity.Inviter;
                dto.InviterName = await accountMicroService.GetNameById(entity.Inviter);

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
    }
}