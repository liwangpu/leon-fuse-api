namespace ApiModel.Entities
{
    public class OrderContentItem : DataBase
    {
        public string ProductId { get; set; }
        public int Num { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        public string Remark { get; set; }
    }
}
