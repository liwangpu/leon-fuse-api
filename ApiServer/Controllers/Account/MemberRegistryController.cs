using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class MemberRegistryController : ListableController<MemberRegistry, MemberRegistryDTO>
    {

        #region 构造函数
        public MemberRegistryController(IRepository<MemberRegistry, MemberRegistryDTO> repository)
        : base(repository)
        {
        }
        #endregion


    }
}