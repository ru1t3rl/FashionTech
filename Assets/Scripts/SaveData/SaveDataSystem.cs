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

    public UnityEvent saveGameEvent, saveDataLoadedEvent;

    public SaveObject loadedSaveData;
    public string filePath;

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
        filePath = Application.dataPath + "/SaveFiles/SaveData.json";

        loadedSaveData = GetSaveFile();

        Debug.Log("LOADED DATA: " + loadedSaveData.playerPosition);

        if (loadedSaveData != null) saveDataLoadedEvent.Invoke();







        var test = File.Create(Application.dataPath + "/SaveFiles/CreationTest9.json");

        test.Close();

        Debug.Log(test);

        SaveObject testSave = CreateNewSaveObject();
        string testString = JsonUtility.ToJson(testSave);

        File.WriteAllText(Application.dataPath + "/SaveFiles/CreationTest9.json", testString);
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
        
        if (!exists || newSaveObject == null)
        {
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
