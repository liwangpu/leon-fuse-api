namespace ApiModel
{
    public interface IDTOTransfer<T>
        where T : IData
    {
        T ToDTO();
    }

}
