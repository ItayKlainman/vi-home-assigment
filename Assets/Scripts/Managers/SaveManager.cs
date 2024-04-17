using Firebase.Database;
using System;
using Services;
using Interfaces;

namespace Managers
{
    public static class SaveManager
    {
        private static readonly ISaveData saveService = new CloudSaveService(FirebaseDatabase.GetInstance("https://vi-home-assignment-default-rtdb.europe-west1.firebasedatabase.app"));

        public static void SaveData(object data)
        {
            saveService.Save(data);
        }

        public static void LoadData<T>(Action<T> OnCompleteLoad)
        {
            saveService.Load<T>(OnCompleteLoad);
            //need to handle case where data cannot be loaded. 
        }
    }
}
