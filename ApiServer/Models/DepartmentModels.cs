using ApiModel.Entities;

namespace ApiServer.Models
{
    /// <summary>
    /// 部门编辑模型
    /// </summary>
    public class DepartmentEditModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string OrganizationId { get; set; }
    }
}
