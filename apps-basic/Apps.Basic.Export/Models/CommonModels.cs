using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Basic.Export.Models
{
    public class IconEdtiModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string ObjId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string AssetId { get; set; }
    }
}
