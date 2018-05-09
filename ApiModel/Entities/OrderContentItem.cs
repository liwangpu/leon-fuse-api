namespace ApiModel.Entities
{
    public class OrderContentItem
    {
        public string ProductSpecId { get; set; }
        public int Num { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecName { get; set; }
    }
}
