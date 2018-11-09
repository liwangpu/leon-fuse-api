using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Models
{
    public class WorkFlowCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string ApplyOrgans { get; set; }
    }

    public class WorkFlowEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string ApplyOrgans { get; set; }
    }

    public class WorkFlowItemEditModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string workFlowId { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubWorkFlowId { get; set; }
        public string OperateRoles { get; set; }
        public int FlowGrade { get; set; }
    }
}
