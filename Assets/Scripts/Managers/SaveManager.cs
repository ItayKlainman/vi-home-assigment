using Firebase.Database;
using System;
using Services;
using Interfaces;

namespace Managers
{
    public static class SaveManager
    {
        private static ISaveData saveService = new CloudSaveService();

        public static void SaveData<T>(T data) where T: class
        {
            saveService.Save(data);
        }

        public static void LoadData<T>(Action<T> OnCompleteLoad)
        {
            saveService.Load(OnCompleteLoad);
        }
    }
}
