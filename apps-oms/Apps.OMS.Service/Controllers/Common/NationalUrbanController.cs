using Apps.Base.Common;
using Apps.Base.Common.Consts;
using Apps.OMS.Export.DTOs;
using Apps.OMS.Export.Models;
using Apps.OMS.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.OMS.Service.Controllers
{
    /// <summary>
    /// 省市区控制器
    /// </summary>
    [AllowAnonymous]
    [Route("/[controller]")]
    [ApiController]
    public class NationalUrbanController : ControllerBase
    {
        protected AppDbContext _Context { get; }

        #region 构造函数
        public NationalUrbanController(AppDbContext context, IOptions<AppConfig> settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据查询获取省市区信息
        /// <summary>
        /// 根据查询获取省市区信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<NationalUrbanDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery]NationalUrbanQueryModel model)
        {
            //如果没有输入省市区的类型,默认查询省信息
            if (string.IsNullOrWhiteSpace(model.NationalUrbanTypes))
                model.NationalUrbanTypes = NationalUrbanTypeConst.Province;

            var query = _Context.NationalUrbans.Select(x => x);
            if (!string.IsNullOrWhiteSpace(model.Name))
                query = query.Where(x => x.Name.Contains(model.Name));
            if (!string.IsNullOrWhiteSpace(model.NationalUrbanTypes))
            {
                var typeArr = model.NationalUrbanTypes.Split(",", StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(x => typeArr.Contains(x.NationalUrbanType));
            }
            if (!string.IsNullOrEmpty(model.ParentId))
                query = query.Where(x => x.ParentId == model.ParentId);

            var nations = await query.ToListAsync();
            if (nations != null)
            {
                var nationsDto = new List<NationalUrbanDTO>();
                foreach (var item in nations)
                {
                    var dto = new NationalUrbanDTO();
                    dto.Id = item.Id;
                    dto.Name = item.Name;
                    dto.ParentId = item.ParentId;
                    dto.NationalUrbanType = item.NationalUrbanType;
                    nationsDto.Add(dto);
                }
                return Ok(nationsDto);
            }
            return NotFound();
        }
        #endregion

        #region Get 根据Id查询省市区信息
        /// <summary>
        /// 根据Id查询省市区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NationalUrbanDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var nation = await _Context.NationalUrbans.FirstOrDefaultAsync(x => x.Id == id);
            if (nation != null)
            {
                var dto = new NationalUrbanDTO();
                dto.Id = nation.Id;
                dto.Name = nation.Name;
                dto.ParentId = nation.ParentId;
                dto.NationalUrbanType = nation.NationalUrbanType;
                var children = await _Context.NationalUrbans.Where(x => x.ParentId == nation.Id).ToListAsync();
                if (children != null)
                {
                    var childrenDto = new List<NationalUrbanDTO>();
                    foreach (var item in children)
                    {
                        var childDto = new NationalUrbanDTO();
                        childDto.Id = item.Id;
                        childDto.Name = item.Name;
                        childDto.ParentId = item.ParentId;
                        childDto.NationalUrbanType = item.NationalUrbanType;
                        childrenDto.Add(childDto);
                    }
                    dto.Children = childrenDto;
                    return Ok(dto);
                }

            }
            return NotFound();
        }
        #endregion
    }
}