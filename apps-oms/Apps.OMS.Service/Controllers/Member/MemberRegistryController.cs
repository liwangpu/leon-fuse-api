using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Consts;
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
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class MemberRegistryController : ServiceBaseController<MemberRegistry>
    {
        protected AppConfig _AppConfig { get; }
        protected AppDbContext _Context { get; }

        #region 构造函数
        public MemberRegistryController(IRepository<MemberRegistry> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository)
        {
            _Context = context;
            _AppConfig = settingsOptions.Value;
        }
        #endregion

        #region Get 根据分页获取会员邀请信息
        /// <summary>
        /// 根据分页获取会员邀请信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MemberRegistryDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);

            var toDTO = new Func<MemberRegistry, Task<MemberRegistryDTO>>(async (entity) =>
            {
                var dto = new MemberRegistryDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.Province = entity.Province;
                dto.City = entity.City;
                dto.Area = entity.Area;
                dto.Phone = entity.Phone;
                dto.Mail = entity.Mail;
                dto.Company = entity.Company;
                dto.Inviter = entity.Inviter;
                dto.InviterName = await accountMicroService.GetNameById(entity.Inviter);
                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                return await Task.FromResult(dto);
            });

            var advanceQuery = new Func<IQueryable<MemberRegistry>, Task<IQueryable<MemberRegistry>>>(async (query) =>
            {
                query = query.Where(x => x.OrganizationId == CurrentAccountOrganizationId && x.IsApprove == false);
                return await Task.FromResult(query);
            });
            return await _PagingRequest(model, toDTO, advanceQuery);
        }
        #endregion

        #region Get 根据Id获取会员邀请信息
        /// <summary>
        /// 根据Id获取会员邀请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MemberRegistryDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);

            var toDTO = new Func<MemberRegistry, Task<MemberRegistryDTO>>(async (entity) =>
            {
                var dto = new MemberRegistryDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.Province = entity.Province;
                dto.City = entity.City;
                dto.Area = entity.Area;
                dto.Phone = entity.Phone;
                dto.Mail = entity.Mail;
                dto.Company = entity.Company;
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

        #region Post 新建会员邀请
        /// <summary>
        /// 新建会员邀请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MemberRegistryDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MemberRegistryCreateModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer);
            var inviteUser = await accountMicroService.GetById(model.Inviter);
            if (inviteUser == null)
            {
                ModelState.AddModelError("Inviter", $"邀请人{model.Inviter}记录不存在");
                return new ValidationFailedResult(ModelState);
            }

            var refMember = await _Context.MemberRegistries.FirstOrDefaultAsync(x => x.Mail == model.Mail.Trim() && x.OrganizationId == inviteUser.OrganizationId);
            var member = refMember != null ? refMember : new MemberRegistry();
            if (string.IsNullOrWhiteSpace(member.Id))
                member.Id = GuidGen.NewGUID();
            member.Name = model.Name;
            member.Description = model.Remark;
            member.ActiveFlag = AppConst.Active;
            member.Phone = model.Phone;
            member.Mail = model.Mail;
            member.Company = model.Company;
            member.Province = model.Province;
            member.City = model.City;
            member.Area = model.Area;
            member.Inviter = model.Inviter;
            member.Creator = model.Inviter;
            member.Modifier = model.Inviter;
            member.OrganizationId = inviteUser.OrganizationId;
            if (refMember == null)
                member.CreatedTime = DateTime.Now;
            member.ModifiedTime = DateTime.Now;
            if (refMember == null)
                _Context.MemberRegistries.Add(member);
            else
                _Context.MemberRegistries.Update(member);
            await _Context.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}