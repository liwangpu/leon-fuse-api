using ApiModel;
using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Controllers.Account
{
    //[Authorize]
    [Route("/[controller]")]
    public class OrganController : ListableController<Organization>
    {
        public OrganController(ApiDbContext context)
            : base(new OrganizationStore(context))
        {



        }

        public override async Task<PagedData<IData>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            return await base.Get(search, page, pageSize, orderBy, desc);
        }
    }
}