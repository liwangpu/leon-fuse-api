using ApiModel;
using ApiServer.Stores;

namespace ApiServer.Controllers.Common
{
    /// <summary>
    ///有很多ICon资源类型的Resource需要替换ICon图标
    ///ListableController提供替换图标的Action
    /// </summary>
    public class ListableController<T> : CommonController<T>
               where T : class, IEntity, IDTOTransfer<IData>, new()
    {
        public ListableController(IStore<T> store)
            : base(store)
        {

        }

    }
}