using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditorInternal;

public class Buttons : MonoBehaviour
{
    private DataPersist dataPersistScript;
    private InputField inputField;

    private void Start()
    {
        dataPersistScript = GameObject.Find("DataPersist").GetComponent<DataPersist>();
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }

    public void startGame() 
    {
        //loadRecord();
        loadRecords();
        
    }

    public void endGame() 
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void loadRecord()
    {
        
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            dataPersistScript.name = data.name;
            dataPersistScript.record = data.record;
        }
        else 
        {
            dataPersistScript.name = inputField.text;
            dataPersistScript.record = 0;
        }
    }

    private void loadRecords() 
    {
        string filePath = Application.persistentDataPath + "/savefile.json";
        List<SaveData> records = new List<SaveData>();

        // Si el archivo existe, lo leemos y deserializamos
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            SaveDataList saveDataList = JsonUtility.FromJson<SaveDataList>(json);
            records = saveDataList.records; // Asignamos la lista de records a la variable

            //Buscar en la lista que coincida con inputfield
            dataPersistScript.name = inputField.text;
            dataPersistScript.record = 0;
            records.ForEach(record =>
            {
                Debug.Log("Record loaded: " + record.name + " " + record.record + " points.");
                if (record.name == inputField.text)
                {
                    Debug.Log("Record match");
                    dataPersistScript.name = record.name;
                    dataPersistScript.record = record.record;
                    loadScene();
                }
            });
            loadScene();

        }
        else 
        {
            dataPersistScript.name = inputField.text;
            dataPersistScript.record = 0;
            loadScene();
        }
    }

    private void loadScene() 
    {
        SceneManager.LoadScene("main");
    }
}
