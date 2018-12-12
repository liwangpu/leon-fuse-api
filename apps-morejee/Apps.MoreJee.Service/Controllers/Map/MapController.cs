using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Export.DTOs;
using Apps.MoreJee.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apps.MoreJee.Service.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MapController : ListviewController<Map>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public MapController(IRepository<Map> repository, AppDbContext context)
            : base(repository)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取用户信息
        /// <summary>
        /// 根据分页获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MapDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var toDTO = new Func<Map, Task<MapDTO>>(async (entity) =>
            {
                var dto = new MapDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        public override Task<IActionResult> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}