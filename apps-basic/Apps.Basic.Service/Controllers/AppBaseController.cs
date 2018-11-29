using Microsoft.AspNetCore.Mvc;

namespace Apps.Basic.Service.Controllers
{
    public class AppBaseController : ControllerBase
    {
        /// <summary>
        /// 获取当前用户Id信息
        /// </summary>
        public string CurrentAccountId
        {
            get
            {
                return User.Identity.Name;
            }
        }
    }
}