namespace ApiModel.Entities
{
    public class OrderContentItem : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public int Num { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        public string Remark { get; set; }
    }
}
