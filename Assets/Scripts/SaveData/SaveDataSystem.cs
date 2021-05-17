using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Valve.Newtonsoft.Json;

public class SaveDataSystem : MonoBehaviour
{
    public static SaveDataSystem instance = null;

    public UnityEvent saveGameEvent;

    public SaveObject loadedSaveData;
    public string filePath;

    void Start()
    {
        if (instance == null)
        {
            
            instance = this;
            Debug.Log("instance given");
            print(instance);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        saveGameEvent = new UnityEvent();

        filePath = Application.dataPath + "/SaveFiles/SaveData.json";

        loadedSaveData = GetSaveFile();

    }

    private SaveObject GetSaveFile()
    {
        bool exists = File.Exists(filePath);

        SaveObject newSaveObject = null;

        if (exists)
        {
            print(1);
            string loadedSaveData = File.ReadAllText(filePath);

            newSaveObject = JsonUtility.FromJson<SaveObject>(loadedSaveData);
        }
        
        if (!exists || newSaveObject == null)
        {
            print(2);
            //TODO: Create new file and save this to path
            newSaveObject = CreateNewSaveObject();
        }

        return newSaveObject;
    }

    private SaveObject CreateNewSaveObject()
    {
        SaveObject saveObject = new SaveObject
        {
            playerPosition = new Vector3(0, 0, 0),
        };

        string saveFile = JsonUtility.ToJson(saveObject);

        return saveObject;
    }

    private void SaveGame()
    {
        Debug.Log("Saving game");
        saveGameEvent.Invoke();
        
        SaveToJson();
    }

    private void SaveToJson()
    {
        Debug.Log("saving to json");

        string saveFile = JsonUtility.ToJson(loadedSaveData);

        File.WriteAllText(filePath, saveFile);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}

public class SaveObject
{
    public Vector3 playerPosition;
}
