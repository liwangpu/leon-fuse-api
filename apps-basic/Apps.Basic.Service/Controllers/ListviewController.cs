using Apps.Base.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    public abstract class ListviewController<T> : ServiceBaseController<T>
          where T : class, IData, new()
    {
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
    }
}