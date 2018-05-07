namespace ApiModel
{
    public interface IData
    {
        string Id { get; set; }
        string Name { get; set; }

        bool IsPersistence();
    }
}
