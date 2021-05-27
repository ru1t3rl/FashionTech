using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Valve.Newtonsoft.Json;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SaveDataSystem : MonoBehaviour
{
    public static SaveDataSystem instance = null;

    public UnityEvent saveGameEvent, saveDataLoadedEvent;

    public SaveObject loadedSaveData;
    public string filePath, saveFileName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        saveGameEvent = new UnityEvent();
        saveDataLoadedEvent = new UnityEvent();
    }

    void Start()
    {
        saveFileName = "SaveData.json";

        filePath = Application.dataPath + "/SaveFiles/" + saveFileName;

        loadedSaveData = GetSaveFile();

        if (loadedSaveData != null) saveDataLoadedEvent.Invoke();
    }

    private SaveObject GetSaveFile()
    {
        bool exists = File.Exists(filePath);

        SaveObject newSaveObject = null;

        if (exists)
        {
            string loadedSaveData = File.ReadAllText(filePath);

            newSaveObject = JsonUtility.FromJson<SaveObject>(loadedSaveData);
        }
        else
        {
            var newSaveFileJSON = File.Create(filePath);
            newSaveFileJSON.Close();

            newSaveObject = CreateNewSaveObject();
            string newSaveDataToWrite = JsonUtility.ToJson(newSaveObject);
            File.WriteAllText(filePath, newSaveDataToWrite);
        }

        if (exists && newSaveObject == null)
        {
            newSaveObject = CreateNewSaveObject();
        }

        return newSaveObject;
    }

    private SaveObject CreateNewSaveObject()
    {
        SaveObject saveObject = new SaveObject
        {
            playerPosition = Player.instance.transform.position,
        };

        string saveFile = JsonUtility.ToJson(saveObject);

        return saveObject;
    }

    private void SaveGame()
    {
        saveGameEvent.Invoke();
        
        SaveToJson();
    }

    private void SaveToJson()
    {
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
