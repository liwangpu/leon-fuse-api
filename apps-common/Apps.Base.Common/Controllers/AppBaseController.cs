using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Apps.Base.Common.Controllers
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

        /// <summary>
        /// 当前用户的内置角色信息
        /// </summary>
        public string CurrentAccountInnerRoleId
        {
            get
            {
                var claim = User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                if (claim != null)
                    return claim.Value;
                return string.Empty;
            }
        }

        /// <summary>
        /// 当前用户组织Id信息
        /// </summary>
        public string CurrentAccountOrganizationId
        {
            get
            {
                var claim = User.Claims.Where(x => x.Type == "OrganizationId").FirstOrDefault();
                if (claim != null)
                    return claim.Value;
                return string.Empty;
            }
        }

        public string Token
        {
            get
            {
                string authorizationStr = Request.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(authorizationStr))
                {
                    var arr = authorizationStr.Split("bearer ", StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length > 0)
                        return arr[0].Trim();
                }
                return string.Empty;
            }
        }
    }
}