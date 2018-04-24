using ApiModel.Entities;
using BambooCore;
using System;

namespace ApiServer.Models
{
    public class AccountModel : IModel<Account>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool Frozened { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
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

        public Account ToEntity()
        {
            var entity = new Account();
            entity.Id = Id;
            entity.Name = Name;
            entity.Mail = Mail;
            if (!string.IsNullOrWhiteSpace(Password))
                entity.Password = Md5.CalcString(Password);
            entity.Phone = Phone;
            entity.Location = Location;
            entity.OrganizationId = OrganizationId;
            entity.Type = Type;
            entity.ExpireTime = ExpireTime;
            entity.ActivationTime = ActivationTime;
            entity.DepartmentId = DepartmentId;
            return entity;
        }
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
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Location { get; set; }
        public string Brief { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
    }

    #region RegisterAccountModel 用户注册模型
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class RegisterAccountModel
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
    }
    #endregion

}
