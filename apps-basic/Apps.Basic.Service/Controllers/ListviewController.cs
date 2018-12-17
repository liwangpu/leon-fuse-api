using Apps.Base.Common.Controllers;
using Apps.Base.Common.Interfaces;
using Apps.Basic.Service.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    public abstract class ListviewController<T> : ServiceBaseController<T>
          where T : class, IData, new()
    {
        protected abstract AppDbContext _Context { get; }

        #region 构造函数
        public ListviewController(IRepository<T> repository)
         : base(repository)
        { }
        #endregion

        #region _BatchDeleteRequest 批量删除数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="afterDeleteLiteral"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _BatchDeleteRequest(string ids, Func<string, Task> afterDeleteLiteral = null)
        {
            var rejectMessages = new List<Dictionary<string, string>>();
            var idArr = ids.Split(",");
            foreach (var id in idArr)
            {
                var deleteMessage = await _Repository.CanDeleteAsync(id, CurrentAccountId);
                if (!string.IsNullOrWhiteSpace(deleteMessage))
                {
                    var dict = new Dictionary<string, string>();
                    dict[id] = deleteMessage;
                    rejectMessages.Add(dict);
                    continue;
                }
                await _Repository.DeleteAsync(id, CurrentAccountId);
                if (afterDeleteLiteral != null)
                    await afterDeleteLiteral(id);
            }
            if (rejectMessages.Count > 0)
                return Ok(rejectMessages);
            return Ok();
        }
        #endregion

        /// <summary>
        /// 更新图标信息
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        //[Route("ChangeICon")]
        //[HttpPut]
        //[ValidateModel]
        //[ProducesResponseType(typeof(ValidationResultModel), 400)]
        //public async Task<IActionResult> ChangeICon([FromBody]IconEdtiModel model)
        //{
        //    var file = await _Context.Files.FirstOrDefaultAsync(x => x.Id == model.AssetId);
        //    var entity = await _Context.Set<T>().FirstOrDefaultAsync(x => x.Id == model.ObjId) as IListView;
        //    if (file == null)
        //    {
        //        ModelState.AddModelError("message", $"找不到文件Id为{model.AssetId}的记录信息");
        //        return BadRequest(ModelState);
        //    }
        //    if (entity == null)
        //    {
        //        ModelState.AddModelError("message", $"找不到对象Id为{model.ObjId}的记录信息");
        //        return BadRequest(ModelState);
        //    }

        //    entity.Icon = file.Id;
        //    _Context.Set<T>().Update(entity);


        //}

    }
}