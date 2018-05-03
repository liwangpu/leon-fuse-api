using System.Collections.Generic;

namespace ApiModel
{
    public delegate void OnToDictionaryHandler<T>(T src, Dictionary<string, object> dicData) where T : IEntity;
    public interface IEntity : IData
    {
        bool IsPersistence();
    }
}
