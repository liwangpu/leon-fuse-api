namespace ApiModel
{
    public class DataBase : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsPersistence()
        {
            if (!string.IsNullOrWhiteSpace(Id))
                return true;
            return false;
        }
    }
}
