using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Models
{
    public class AccountModel
    {
        public string Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool Frozened { get; set; }
        /// <summary>
        /// 账号类型，系统管理员，普通用户，供应商，设计公司等等
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 账号有效期，登陆时间小于这个有效期则无法登陆
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 账号启用时间，如果当前登陆时间小于启用时间，则不能登陆。
        /// </summary>
        public DateTime ActivationTime { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
    }

    public class NewPasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AccountProfileModel
    {
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Location { get; set; }
        public string Brief { get; set; }
    }

    public class RegisterAccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }




}
