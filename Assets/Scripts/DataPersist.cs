using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersist : MonoBehaviour
{
    public static DataPersist instance;

    public string name;
    public int record;
    

    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
