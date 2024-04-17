using Firebase.Database;
using UnityEngine.Device;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class CloudSaveService : ISaveData
{
    private DatabaseReference dbRef;

    private readonly string userID = SystemInfo.deviceUniqueIdentifier;
    private readonly string parentObjectID = "users";

    public CloudSaveService(FirebaseDatabase firebaseDatabase)
    {
        dbRef = firebaseDatabase.RootReference;
    }

    public async void Save(ISavableData saveData)
    {
        var saveDataID = saveData.GetType().Name;
        var serializedData = Serialize(saveData);

        await dbRef.Child(parentObjectID).Child(userID).Child(saveDataID).SetRawJsonValueAsync(serializedData).ContinueWith(task => {

            if(task.IsCompletedSuccessfully)
            {
                Debug.WriteLine($"Loaded data of type {saveDataID} successfully.");
                return;
            }

            else
            {
                HandleFailedRequest(task, $"Failed to load data of type {saveDataID}");
            }
        });
    }

    public void Load<T>(Action<T> onComplete) where T : ISavableData
    {
        var loadDataID = typeof(T).Name;

        dbRef.Child(parentObjectID).Child(userID).Child(loadDataID).GetValueAsync().ContinueWith(task => {

            if(task.IsCompletedSuccessfully)
            {
                onLoadSuccessful(onComplete, task, loadDataID);
                return;
            }
            else
            {
                HandleFailedRequest(task, $"Failed to load data of type {loadDataID}");
            }
        });
    }

    private void onLoadSuccessful<T>(Action<T> onComplete, Task<DataSnapshot> task, string loadedDataType) where T : ISavableData
    {
        var rawJson = task.Result.GetRawJsonValue();
        var deserializedData = Deserialize<T>(rawJson);

        onComplete.Invoke(deserializedData);

        Debug.WriteLine($"Loaded data of type {loadedDataType} successfully.");
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

    private string Serialize(ISavableData data)
    {
        // Implement serialization logic here
        return UnityEngine.JsonUtility.ToJson(data);
    }

    private T Deserialize<T>(string json)
    {
        // Implement deserialization logic here
        return UnityEngine.JsonUtility.FromJson<T>(json);
    }
}
