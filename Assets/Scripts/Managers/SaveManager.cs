using System;
using Services;
using Interfaces;

namespace Managers
{
    public static class SaveManager
    {
        private static ISaveData saveService = new CloudSaveService();

        public static void SaveData<T>(T data,string key) where T: class
        {
            saveService.Save(data, key);
        }

        public static void LoadData<T>(string key, Action<T> OnCompleteLoad)
        {
           saveService.Load(key, OnCompleteLoad);
        }
    }
}
