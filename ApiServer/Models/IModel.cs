using ApiModel;
namespace ApiServer.Models
{
    public interface IModel<T>
        where T : IData
    {
        T ToEntity();
    }
}
