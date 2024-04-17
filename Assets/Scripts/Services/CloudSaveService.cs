using Firebase.Database;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Interfaces;

namespace Services
{
    public class CloudSaveService : ISaveData
    {
        private DatabaseReference dbRef;

        private readonly string userID = SystemInfo.deviceUniqueIdentifier;
        private readonly string parentObjectID = "users";

        public CloudSaveService()
        {
            dbRef = FirebaseDatabase.GetInstance("https://vi-home-assignment-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
        }

        public async void Save<T>(T saveData) where T : class
        {
            var saveDataID = typeof(T).Name;
            var serializedData = Serialize(saveData);

            await dbRef.Child(parentObjectID).Child(userID).Child(saveDataID).SetRawJsonValueAsync(serializedData).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log($"Loaded data of type {saveDataID} successfully.");
                }
                else
                {
                    HandleFailedRequest(task, $"Failed to save data of type {saveDataID}");
                }
            });
        }

        public void Load<T>(Action<T> onComplete)
        {
            var loadDataID = typeof(T).Name;

            dbRef.Child(parentObjectID).Child(userID).Child(loadDataID).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var rawJson = task.Result.GetRawJsonValue();
                    var deserializedData = Deserialize<T>(rawJson);

                    onComplete.Invoke(deserializedData);
                    Debug.Log($"Loaded data of type {loadDataID} successfully.");
                }
                else
                {
                    HandleFailedRequest(task, $"Failed to load data of type {loadDataID}");
                }
            });
        }

        private void HandleFailedRequest(Task task, string failedMessage)
        {
            if (task.IsFaulted)
            {
                foreach (var exception in task.Exception.InnerExceptions)
                {
                    Debug.LogError($"Error: {exception.Message}");
                }
            }
            else if (task.IsCanceled)
            {
                Debug.LogWarning("Operation canceled.");
            }

            Debug.LogError(failedMessage);
        }

        private string Serialize<T>(T data) where T : class
        {
            // Implement custom serialization logic here
            return JsonUtility.ToJson(data);
        }

        private T Deserialize<T>(string json)
        {
            // Implement custom deserialization logic here
            return JsonUtility.FromJson<T>(json);
        }
    }
}
