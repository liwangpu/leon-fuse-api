using Apps.Base.Common.Interfaces;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 文件资源控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ServiceBaseController<FileAsset>
    {
        #region 构造函数
        public FilesController(IRepository<FileAsset> repository)
          : base(repository)
        {
        }
        #endregion

        public override Task<IActionResult> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}