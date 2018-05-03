using ApiModel;
using ApiServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class PermissionStore<T> : StoreBase<T>
               where T : class, IEntity, ApiModel.ICloneable, new()
    {
        #region 构造函数
        public PermissionStore(ApiDbContext context)
        : base(context)
        {

        }
        #endregion



    }
}
