using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class SolutionStore : ListableStore<Solution, SolutionDTO>, IStore<Solution, SolutionDTO>
    {

        #region 构造函数
        public SolutionStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Solution data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrEmpty(data.LayoutId))
            {
                var exist = await _DbContext.Layouts.CountAsync(x => x.Id == data.LayoutId && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
                if (!exist)
                    modelState.AddModelError("LayoutId", "没有找到该记录信息");
            }
        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdateAsync(string accid, Solution data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrEmpty(data.LayoutId))
            {
                var exist = await _DbContext.Layouts.CountAsync(x => x.Id == data.LayoutId && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
                if (!exist)
                    modelState.AddModelError("LayoutId", "没有找到该记录信息");
            }
        }
        #endregion



    }
}
