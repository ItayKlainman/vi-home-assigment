using Firebase.Database;
using UnityEngine.Device;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

public class CloudSaveService : ISaveData
{
    private DatabaseReference reference;
    private string id;

    public CloudSaveService()
    { 
        reference = FirebaseDatabase.GetInstance("https://vi-home-assignment-default-rtdb.europe-west1.firebasedatabase.app").RootReference;
        id = SystemInfo.deviceUniqueIdentifier;
    }

    public async void Save(ISavableData saveData)
    {
        var saveDataID = saveData.GetType().Name;  //SaveID
        var json = UnityEngine.JsonUtility.ToJson(saveData);

        await reference.Child("players").Child(id).Child(saveDataID).SetRawJsonValueAsync(json).ContinueWith(task => {

            if(task.IsCompletedSuccessfully)
            {
                Debug.WriteLine($"Loaded data of type {saveDataID} successfully.");
                return;
            }

            foreach (var execption in task.Exception.InnerExceptions)
            {
                Debug.WriteLine($"Failed to load data {saveDataID}. reason: {execption.Message}");
                //handle exeptions here. also encapsulate and clean up for load function.
            }
        });
    }

    public void Load<T>(Action<T> onComplete) where T : ISavableData
    {
       reference.Child("players").Child(id).GetValueAsync().ContinueWith(task => {

        if(task.IsCompletedSuccessfully)
            {
               var rawJson = task.Result.GetRawJsonValue();
               var deserializedData = UnityEngine.JsonUtility.FromJson<T>(rawJson);

               onComplete.Invoke(deserializedData);

               return;
           }
           else
           {
               foreach (var execption in task.Exception.InnerExceptions)
               {
                   //handle exeptions here. also encapsulate and clean up for save function.
               }
           }
         });
    }


}
