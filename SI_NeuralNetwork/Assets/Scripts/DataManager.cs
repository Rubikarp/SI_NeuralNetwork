using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System;

[XmlRoot("DataNets")]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string path;


    XmlSerializer serializer = new XmlSerializer(typeof(Data));
    Encoding encoding = Encoding.GetEncoding("UTF-8");


    public void Awake()
    {
        instance = this;
        SetPath();
    }

    private void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "Data.xml");
    }

    public void Save(Data data)
    {
        StreamWriter srteamwritter = new StreamWriter(path, false, encoding);
        serializer.Serialize(srteamwritter, data);
    }

    public Data Load()
    {
        if (File.Exists(path))
        {
            FileStream fliestream = new FileStream(path, FileMode.Open);

            return serializer.Deserialize(fliestream) as Data;
        }
        else
        {
            return null;
        }
    }
}
