using Apps.Base.Common.Interfaces;
using System;

namespace Apps.MoreJee.Data.Entities
{
    public class ProductSpec : IEntity, IListView
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
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string Modifier { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
        /// <summary>
        /// 数据激活状态标记
        /// </summary>
        public int ActiveFlag { get; set; }
        /// <summary>
        /// 组织Id
        /// </summary>
        public string OrganizationId { get; set; }
        public string Icon { get; set; }
        /// <summary>
        /// 零售价，单位为元
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 合伙人价格|渠道价，单位为元
        /// </summary>
        public decimal PartnerPrice { get; set; }
        /// <summary>
        /// 进货价，单位为元
        /// </summary>
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// 第三方ID，此产品在供应商自己的系统比如ERP的ID
        /// </summary>
        public string TPID { get; set; }
        public string Components { get; set; }
        public string StaticMeshs { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
