using BambooCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Account : EntityBase, IListable, ICloneable, IDTOTransfer<AccountDTO>
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool MailValid { get; set; }
        public bool PhoneValid { get; set; }
        public bool Frozened { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
        public string Location { get; set; }
        /// <summary>
        /// 账号类型，系统管理员，普通用户，供应商，设计公司等等
        /// </summary>
        public string Type
        {
            get; set;
        }
        /// <summary>
        /// 账号有效期，登陆时间小于这个有效期则无法登陆
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 账号启用时间，如果当前登陆时间小于启用时间，则不能登陆。
        /// </summary>
        public DateTime ActivationTime { get; set; }
        [InverseProperty("Owner")]
        public Organization Organization { get; set; }
        public Department Department { get; set; }
        public List<ClientAsset> ClientAssets { get; set; }
        public List<Product> Products { get; set; }
        public List<Solution> Solutions { get; set; }
        public List<Layout> Layouts { get; set; }
        public List<Order> Orders { get; set; }
        public List<AssetFolder> Folders { get; set; }
        public List<FileAsset> Files { get; set; }
        public List<AccountOpenId> OpenIds { get; set; }


        /// <summary>
        /// 权限记录，记录能访问的所有类型资源的所有权限设置。不在此列出的则无法访问。
        /// </summary>

        public List<PermissionItem> Permissions { get; set; }

        public AccountDTO ToDTO()
        {
            var dto = new AccountDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.Mail = Mail;
            dto.Location = Location;
            dto.OrganizationId = OrganizationId;
            dto.DepartmentId = DepartmentId;
            dto.Phone = Phone;
            dto.Frozened = Frozened;
            dto.Type = Type;
            dto.ExpireTime = ExpireTime;
            dto.ActivationTime = ActivationTime;
            return dto;
        }
    }


    public class AccountDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Mail { get; set; }
        public string Location { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
        public string Phone { get; set; }
        public bool Frozened { get; set; }
        public string Type { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
    }
}
