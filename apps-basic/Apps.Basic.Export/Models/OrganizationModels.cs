using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Basic.Export.Models
{
    public class OrganizationCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string OrganizationTypeId { get; set; }
        public string Description { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public string Location { get; set; }
    }

    public class OrganizationEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string OrganizationTypeId { get; set; }
        public string Description { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public string Location { get; set; }
    }
}
