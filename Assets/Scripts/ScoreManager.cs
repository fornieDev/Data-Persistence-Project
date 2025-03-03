using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private string name;
    private DataPersist dataPersistScript;
    private Text bestScoreText;
    private int points;
    private MainManager mainManagerScript;

    //public GameObject bestScoreTextGameObject;
    //public TextMesh bestScoreTextPublic;

    // Start is called before the first frame update
    void Start()
    {
        mainManagerScript = GameObject.Find("MainManager").GetComponent<MainManager>();

        dataPersistScript = GameObject.Find("DataPersist").GetComponent<DataPersist>();
        name = dataPersistScript.name;
        Debug.Log("Name recovered :" + name);

        bestScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best Score : " + name + " : " + dataPersistScript.record;
            //Esto se tendria que cambiar el 0 por el record

        }
        else
        {
            Debug.Log("No se encontro bestScoreText");
        }

        //bestScoreTextPublic = bestScoreTextGameObject.GetComponent<TextMesh>();
        //bestScoreText.text = name;



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setRecord()
    {
        //Set record if is greater
        points = mainManagerScript.m_Points;
        if (points > dataPersistScript.record)
        {
            bestScoreText.text = "Best Score : " + name + " : " + points;
            dataPersistScript.record = points;
            //Save record in json
            //saveRecord();
            saveRecords();
        }




    }

    private void saveRecord()
    {
        SaveData saveData = new SaveData();
        saveData.name = name;
        saveData.record = points;

        string json1 = JsonUtility.ToJson(saveData);

        //esto no sobreescribira todos los records?
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json1);
    }

    private void saveRecords()
    {
        //Casos de uso: no existe save file, se bate el record, no se bate el record,se crea un record.
        bool fileExist;
        bool isNewPlayer = true;
        bool recordIsGreater = false;

        //Se guarda la puntuación actual
        SaveData saveData = new SaveData();
        saveData.name = name;
        saveData.record = points;

        //Se busca el arvhivo si existe
        string filePath = Application.persistentDataPath + "/savefile.json";
        SaveDataList savedDataList;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            savedDataList = JsonUtility.FromJson<SaveDataList>(json);
            fileExist = true;
        }
        else
        {
            savedDataList = new SaveDataList();
            savedDataList.records = new List<SaveData>();
            savedDataList.records.Add(saveData);
            fileExist = false;
            //Guardamos y salimos
            string newJson = JsonUtility.ToJson(savedDataList);
            File.WriteAllText(filePath, newJson);
            return;
        }


        //Se convierte para poder iterar
        List<SaveData> recordsList = new List<SaveData>();
        recordsList = savedDataList.records;

        SaveData oldRecord = null;

        recordsList.ForEach(record =>
        {
            if (record.name == name) 
            {
                isNewPlayer = false;
            }
            if (record.name == name && record.record < points)
            {
                //record superado
                recordIsGreater = true;
                oldRecord = record;
            }
        });

        if (isNewPlayer)
        {
            
            savedDataList.records.Add(saveData);
            string addedJson = JsonUtility.ToJson(savedDataList);
            File.WriteAllText(filePath, addedJson);
        }
        else if(recordIsGreater)
        {
            
            savedDataList.records.Remove(oldRecord);
            savedDataList.records.Add(saveData);
            string updatedJson = JsonUtility.ToJson(savedDataList);
            File.WriteAllText(filePath, updatedJson);
        }
    }
}
