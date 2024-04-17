using Firebase.Database;
using UnityEngine.Device;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Interfaces;

namespace Services
{
    public class CloudSaveService : ISaveData
    {
        private DatabaseReference dbRef;

        private readonly string userID = SystemInfo.deviceUniqueIdentifier;
        private readonly string parentObjectID = "users";

        public CloudSaveService(FirebaseDatabase firebaseDatabase)
        {
            dbRef = firebaseDatabase.RootReference;
        }

        public async void Save<T>(T saveData) where T : class
        {
            var saveDataID = typeof(T).Name;
            var serializedData = Serialize(saveData);

            await dbRef.Child(parentObjectID).Child(userID).Child(saveDataID).SetRawJsonValueAsync(serializedData).ContinueWith(task =>
            {

                if (task.IsCompletedSuccessfully)
                {
                    Debug.WriteLine($"Loaded data of type {saveDataID} successfully."); //create unity debugger?
                    return;
                }

                else
                {
                    HandleFailedRequest(task, $"Failed to load data of type {saveDataID}");
                }
            });
        }

        public void Load<T>(Action<T> onComplete, Func<T> createDefault = null)
        {
            var loadDataID = typeof(T).Name;

            dbRef.Child(parentObjectID).Child(userID).Child(loadDataID).GetValueAsync().ContinueWith(task =>
            {

                if (task.IsCompletedSuccessfully)
                {
                    var rawJson = task.Result.GetRawJsonValue();
                    var deserializedData = Deserialize<T>(rawJson);

                    if (deserializedData != null)
                    {
                        onComplete.Invoke(deserializedData);
                        Debug.WriteLine($"Loaded data of type {loadDataID} successfully.");
                    }
                    else
                    {
                        // If data is not found, create a default instance
                        if (createDefault != null)
                        {
                            onComplete.Invoke(createDefault());
                        }
                        else
                        {
                            Debug.WriteLine($"Failed to load data of type {loadDataID}, and no default creator provided.");
                        }

                    }
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
                    Debug.WriteLine($"Error: {exception.Message}");
                }
            }
            else if (task.IsCanceled)
            {
                Debug.WriteLine("Operation canceled.");
            }

            Debug.WriteLine(failedMessage);
        }

        private string Serialize<T>(T data) where T : class
        {
            // Implement custom serialization logic here
            return UnityEngine.JsonUtility.ToJson(data);
        }

        private T Deserialize<T>(string json)
        {
            // Implement custom deserialization logic here
            return UnityEngine.JsonUtility.FromJson<T>(json);
        }
    }
}
