using Firebase.Database;
using System;

public static class SaveManager
{
    private static readonly ISaveData saveService = new CloudSaveService(FirebaseDatabase.GetInstance("https://vi-home-assignment-default-rtdb.europe-west1.firebasedatabase.app"));

    public static void SaveData(ISavableData data)
    {
       saveService.Save(data);
    }

    public static void LoadData<T>(Action<T> OnCompleteLoad) where T: ISavableData
    {
       saveService.Load<T>(OnCompleteLoad);
        //need to handle case where data cannot be loaded. 
    }
}
