using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;
public static class ConfigRead
{
    private static string configString = "{\"DataStation\":[\n  {\n    \"StationName\": \"Station1\",\n    \"Datastage\": [\n      {\n        \"StageName\": \"Plane1\",\n        \"Agrs\": {\n          \"Order\": 1,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"Plane2\",\n        \"Agrs\": {\n          \"Order\": 2,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      }, \n      {\n        \"StageName\": \"Plane3\",\n        \"Agrs\": {\n          \"Order\": 3,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      }, \n      {\n        \"StageName\": \"Plane4\",\n        \"Agrs\": {\n          \"Order\": 4,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      }, \n      {\n        \"StageName\": \"Plane5\",\n        \"Agrs\": {\n          \"Order\": 5,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"Stage3\",\n        \"Agrs\": {\n          \"Order\": 0,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      }\n    ]\n  }\n ]\n}";
    public static RootObject configData = JsonUtility.FromJson<RootObject>(configString);
    public static bool metaOnline = true;

    public static IEnumerator LoadJSONFileConfig(string filePath)
    {
        string jsonString;
        #if UNITY_ANDROID && !UNITY_EDITOR
            using (WWW reader = new WWW(filePath)){
                yield return reader;
                jsonString = reader.text;
            }
        #else
            jsonString = File.ReadAllText(filePath);
        #endif
        configData = JsonUtility.FromJson<RootObject>(jsonString);
        yield return null;
    }
}

[System.Serializable]
public class Agrs
{
    public int Order;
    public float MarginX;
    public float MarginY;
    public float foreground2backgroundRatio;
}

[System.Serializable]
public class Datastage
{
    public string StageName;
    public Agrs Agrs;
}

[System.Serializable]
public class DataStation
{
    public string StationName;
    public List<Datastage> Datastage;
}

[System.Serializable]
public class RootObject
{
    public List<DataStation> DataStation;
}