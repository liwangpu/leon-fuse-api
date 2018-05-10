using ApiModel;
using ApiServer.Stores;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 有很多ICon资源类型的Resource需要替换ICon图标
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="U">Post新建实体对象</typeparam>
    public class ListableController<T, U> : CommonController<T, U>
               where T : class, IEntity, IDTOTransfer<IData>, new()
           where U : class, new()
    {
        public ListableController(IStore<T> store)
            : base(store)
        {

        }

    }
}