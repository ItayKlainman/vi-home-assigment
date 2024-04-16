using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class SaveManager
{
    private static readonly ISaveData saveService = new CloudSaveService();

    public static void SaveData(ISavableData data)
    {
       saveService.Save(data);
    }

    public static void LoadData<T>(Action<T> OnCompleteLoad) where T: ISavableData
    {
       saveService.Load<T>(OnCompleteLoad);
    }
}
