using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class SaveDataSystem : MonoBehaviour
{
    public TextAsset saveDataFile;
    public TextAsset defaultSaveDataFile;

    void Start()
    {
        GetFileWithPath();
    }

    private void GetFileWithPath()
    {
        string filePath = Application.dataPath + "/SaveFiles/SaveData.json";
        bool exists = File.Exists(filePath);

        string loadedSaveData = File.ReadAllText(filePath);

        SaveObject newSaveObject = JsonUtility.FromJson<SaveObject>(loadedSaveData);

        //newSaveObject.testString = loadedSaveData.

        print(filePath);
        print(exists);
        print(loadedSaveData);
        print(newSaveObject.testVariable + "  " + newSaveObject.testString);
    }

    private void CreateSaveObject()
    {
        SaveObject saveObject = new SaveObject
        {
            testVariable = 3,
            testString = "I am a saved string",
        };

        string saveFile = JsonUtility.ToJson(saveObject);

        Debug.Log(saveFile);
    }
}

public class SaveObject
{
    public int testVariable;
    public string testString;
}
