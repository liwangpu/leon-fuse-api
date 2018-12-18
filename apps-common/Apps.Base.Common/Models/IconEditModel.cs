using System.ComponentModel.DataAnnotations;

namespace Apps.Base.Common.Models
{
    public class IconEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string ObjId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string AssetId { get; set; }
    }
}
