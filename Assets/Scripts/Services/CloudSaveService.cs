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

        private const string PARENT_OBJECT_ID = "users";
        private const string DB_URL = "https://vi-home-assignment-default-rtdb.europe-west1.firebasedatabase.app/";
        private readonly string userID = SystemInfo.deviceUniqueIdentifier;

        public CloudSaveService()
        {
            dbRef = FirebaseDatabase.GetInstance(DB_URL).RootReference;
        }

        public async void Save<T>(T saveData, string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Data key was not provided.");
                return;
            }

            var serializedData = Serialize(saveData);

            await dbRef.Child(PARENT_OBJECT_ID).Child(userID).Child(key).SetRawJsonValueAsync(serializedData).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log($"Loaded data of type {key} successfully.");
                }
                else
                {
                    HandleFailedRequest(task, $"Failed to save data of type {key}");
                }
            });
        }

        public void Load<T>(string key, Action<T> onComplete)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Data key was not provided.");
                return;
            }

             dbRef.Child(PARENT_OBJECT_ID).Child(userID).Child(key).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var rawJson = task.Result.GetRawJsonValue();
                    var deserializedData = Deserialize<T>(rawJson);

                    onComplete.Invoke(deserializedData);
                    Debug.Log($"Loaded data of type {key} successfully.");
                }
                else
                {
                    HandleFailedRequest(task, $"Failed to load data of type {key}");
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
