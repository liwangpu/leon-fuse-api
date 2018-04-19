using ApiModel;
namespace ApiServer.Models
{
    public interface IModel<T>
        where T : IEntity
    {
        T ToEntity();
    }
}
