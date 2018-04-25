using System.Collections.Generic;
using System.Linq;
namespace ApiModel
{
    public static class EntityExtention
    {
        #region 将数据对象转换为字典数据
        public static Dictionary<string, object> ToDictionary<TEntity>(this TEntity srcData, OnToDictionaryHandler<TEntity> callback, bool bContainFk = true) where TEntity : IEntity
        {
            var dicData = srcData.ToDictionary();
            if (callback != null)
            {
                callback(srcData, dicData);
            }
            return dicData;
        }

        public static IEnumerable<Dictionary<string, object>> ToDictionary<TEntity>(this IEnumerable<TEntity> datas, OnToDictionaryHandler<TEntity> callback = null, bool bContainFk = true) where TEntity : IEntity
        {
            return datas.Select(data => data.ToDictionary(callback, bContainFk)).ToList();
        }
        #endregion

        public static IEnumerable<IData> ToDTO<TEntity>(this IEnumerable<TEntity> datas) where TEntity : IDTOTransfer<IData>
        {
            return datas.Select(data => data.ToDTO()).ToList();
        }
    }
}
