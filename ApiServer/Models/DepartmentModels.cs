using ApiModel.Entities;

namespace ApiServer.Models
{
    /// <summary>
    /// 部门编辑模型
    /// </summary>
    public class DepartmentEditModel : IModel<Department>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrganizationId { get; set; }
        public Department ToEntity()
        {
            var entity = new Department();
            entity.Id = Id;
            entity.Name = Name;
            entity.Description = Description;
            entity.OrganizationId = OrganizationId;
            return entity;
        }
    }
}
