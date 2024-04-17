using System;

namespace Interfaces
{
    public interface ISaveData
    {
        void Save<T>(T data) where T : class;

        void Load<T>(Action<T> onComplete);
    }
}
