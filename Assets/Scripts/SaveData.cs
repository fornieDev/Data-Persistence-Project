using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SaveData
{
    public string name;
    public int record;


}

[System.Serializable]
class SaveDataList
{
    public List<SaveData> records;
}
