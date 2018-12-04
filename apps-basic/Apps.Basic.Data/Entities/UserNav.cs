using Apps.Base.Common.Interfaces;
using System.Collections.Generic;

namespace Apps.Basic.Data.Entities
{
    public class UserNav : IData
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        public List<UserNavDetail> UserNavDetails { get; set; }
    }
}
