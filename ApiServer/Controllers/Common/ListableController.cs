using ApiModel;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 有很多ICon资源类型的Resource需要替换ICon图标
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="DTO">DTO实体对象</typeparam>
    public class ListableController<T, DTO> : CommonController<T, DTO>
               where T : class, IListable, IDTOTransfer<DTO>, new()
          where DTO : class, IData, new()
    {
        #region 构造函数
        public ListableController(IStore<T, DTO> store)
          : base(store)
        {

        }
        #endregion

        #region ChangeICon 更新图标信息
        /// <summary>
        /// 更新图标信息
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("ChangeICon")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        {
            var mapping = new Func<T, Task<T>>(async (spec) =>
            {
                spec.Icon = icon.AssetId;
                return await Task.FromResult(spec);
            });
            return await _PutRequest(icon.ObjId, mapping);
        }
        #endregion

    }
}