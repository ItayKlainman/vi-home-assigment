using System;

public interface ISaveData
{
    void Save(ISavableData data);

    void Load<T>(Action<T> onComplete) where T : ISavableData;
}
