using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers.Navigation
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NavigationController : ServiceController<Account>
    {
    }
}