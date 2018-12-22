using Apps.Base.Common.Interfaces;
using System;

namespace Apps.MoreJee.Data.Entities
{
    public class ProductGroup : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int ActiveFlag { get; set; }
        public string OrganizationId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 轴心的精确位置
        /// </summary>
        public string PivotLocation { get; set; }
        /// <summary>
        /// 轴心九个点位枚举，TopLeft, TopCenter, TopRight, CenterLeft, Center, CenterRight, BottomLeft...
        /// </summary>
        public int PivotType { get; set; }
        /// <summary>
        /// 朝向的枚举， Forward, Back, Left, Right.  前后左右。 actor的正前方位Forward
        /// </summary>
        public int Orientation { get; set; }
        /// <summary>
        /// 结构为 List<ProductGroupItem>
        /// </summary>
        public string Items { get; set; }
    }
}
