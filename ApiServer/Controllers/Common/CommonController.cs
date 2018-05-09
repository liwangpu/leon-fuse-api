using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using ApiServer.Data;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers.Common
{
    public class CommonController<T> : Controller
         where T : class, IEntity, IDTOTransfer<IData>, new()
    {
        protected bool _RequestValid;
        protected int _StatusCode;
        protected IStore<T> _Store;
        public CommonController(IStore<T> store)
        {
            _Store = store;
        }

        public virtual async Task<PagedData<IData>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            var accid = AuthMan.GetAccountId(this);
            var result = await _Store.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, search);
            return StoreBase<T>.PageQueryDTOTransfer(result);
        }
    }
}