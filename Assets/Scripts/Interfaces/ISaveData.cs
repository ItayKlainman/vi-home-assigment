using System;

namespace Interfaces
{
    public interface ISaveData
    {
        void Save<T>(T data, string key) where T : class;

        void Load<T>(string key, Action<T> onComplete);
    }
}
