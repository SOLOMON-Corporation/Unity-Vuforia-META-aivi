using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
// using Newtonsoft.Json.Linq.JObject;
using System.Collections;
using System;
public static class ConfigRead
{
    // public static string configFile= Path.Combine(Application.streamingAssetsPath,@"Config/InputParameter.json");
    // public static string configFile;
    // public static string projectFile = Path.Combine(Application.streamingAssetsPath,$"Config/{sta}");
    //C:\Users\kai_nguyen\Station1Demo\Assets\StreamingAssets\Vuforia\demo.xml
    private static string configString = "{\"DataStation\":[\n  {\n    \"StationName\": \"Station1\",\n    \"Datastage\": [\n      {\n        \"StageName\": \"140M-C2\",\n        \"Agrs\": {\n          \"Order\": 2,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"100-C_main\",\n        \"Agrs\": {\n          \"Order\": 1,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"Stage3\",\n        \"Agrs\": {\n          \"Order\": 3,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      }\n    ]\n  },\n  {\n    \"StationName\": \"StationUS\",\n    \"Datastage\": [\n      {\n        \"StageName\": \"front.lower\",\n        \"Agrs\": {\n          \"Order\": 1,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"BtnEmergency\",\n        \"Agrs\": {\n          \"Order\": 2,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"BtnLocalRemote\",\n        \"Agrs\": {\n          \"Order\": 3,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"BtnPurge\",\n        \"Agrs\": {\n          \"Order\": 4,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"Motor\",\n        \"Agrs\": {\n          \"Order\": 5,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      },\n      {\n        \"StageName\": \"None\",\n        \"Agrs\": {\n          \"Order\": 0,\n          \"MarginX\": 0.29,\n          \"MarginY\": 0.39,\n          \"foreground2backgroundRatio\": 0.9,\n          \"ObjectH\": 9\n        }\n      }\n    ]\n  }\n ]\n}";
    // public static RootObject configData = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(configFile));
    public static RootObject configData = JsonConvert.DeserializeObject<RootObject>(configString);
    // public static void GetConfig()
    // {
    //     // Load the JSON file from disk
    //     // string configPath = Path.Combine(Application.streamingAssetsPath, configFile);
    //     string configRead = File.ReadAllText(configFile);
    //     Debug.Log("Config contents" + configRead);
    //     // Parse the JSON data into a Config object
    //     configData = JsonUtility.FromJson<DataStation>(configRead);
    //     // return configData;
    // }
    // Path.Combine(Application.streamingAssetsPath, "InputParameter.json");
    public static ProjectData projectData;
    public static string logDisplay = "Log initialize.........";
    // private static WWW reader;
    // public static ProjectData ProjectData
    // {
    //     get { return projectData; }
    //     set
    //     {
    //         projectData = value;
    //         OnFunctionIndexChange?.Invoke(functionIndex);
    //     }
    // }


    // public static IEnumerator LoadJSONFileConfig(string filePath)
    // {
    //     // string filePath = Path.Combine(Application.streamingAssetsPath, "InputParameter.json");
    //     string jsonString;
    //     yield return new WaitForSeconds(1);
    //     #if UNITY_ANDROID
    //         // using (WWW reader = new WWW(filePath)){
    //         //     yield return reader;
    //         //     jsonString = reader.text;
    //         // }

    //         // reader.Dispose();
    //         // www www = UnityWebRequest.Get(filePath);
    //         // yield return www.SendWebRequest();
    //         // jsonString = www.downloadHandler.text;

    //         using (StreamReader reader = new StreamReader(filePath))
    //         {
    //             jsonString = reader.ReadToEnd();
    //         }

    //     #else
    //         jsonString = File.ReadAllText(filePath);
    //     #endif
    //     configData = JsonConvert.DeserializeObject<RootObject>(jsonString);
    //     // dynamic dynaConfigData = JsonConvert.DeserializeObject(jsonString);
    //     // Debug.Log("   ");
    //     yield return new WaitForSeconds(1);
    // }
    public static IEnumerator LoadJSONFileProject(string filePath)
    {
        // string filePath = Path.Combine(Application.streamingAssetsPath, "InputParameter.json");
        string jsonStrin;
        #if UNITY_ANDROID && !UNITY_EDITOR
            // WWW reader = new WWW(filePath);
            // yield return reader;
            // jsonStrin = reader.text;
            

            // reader.Dispose();
            // Debug.Log(jsonString)
            // UnityWebRequest www = UnityWebRequest.Get(filePath);
            // yield return www.SendWebRequest();
            // jsonString = www.downloadHandler.text;
            // using (StreamReader reader = new StreamReader(filePath))
            // {
            //     jsonStrin = reader.ReadToEnd();
            // }
            jsonStrin = "{\"project_name\": \"0413\", \"project_id\": 1681357583, \"project_date\": \"2023/04/13-11:46\", \"project_rule\": {\"State_1\": {\"sequence_number\": 1, \"type\": \"Detection\", \"state_id\": 833822, \"tool\": \"InstanceSeg\", \"model\": {\"model_id\": 164, \"name\": \"0413\", \"help\": [{\"True\": \"OK\", \"False\": \"NG\"}], \"class_name\": [{\"index\": 0, \"name\": \"1_line_ok\", \"color\": \"#0dd235\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 1, \"name\": \"2_EMC_push\", \"color\": \"#ea0514\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 2, \"name\": \"2_EMC_pull\", \"color\": \"#04c958\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 3, \"name\": \"3_Local\", \"color\": \"#0f9a32\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 4, \"name\": \"3_Auto\", \"color\": \"#ea2432\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 5, \"name\": \"4_Purge_on\", \"color\": \"#0fce65\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 6, \"name\": \"4_Purge_off\", \"color\": \"#e00643\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 7, \"name\": \"5_Run\", \"color\": \"#0cc756\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 8, \"name\": \"5_Stop\", \"color\": \"#de0e23\", \"price\": \"\", \"quantity\": \"\"}], \"mode\": \"validation\", \"config\": [{\"show_bbox\": true, \"show_kpts\": false, \"show_mask\": true}]}, \"is_rule\": true, \"show_doc_first\": false, \"documents\": [229], \"document_path\": [\"models/InstanceSeg_0413/slides/InstanceSeg_0413_1.png\"], \"rules\": [{\"order\": 0, \"title\": \"If (CLASS_1_line_ok == 1 )_AS_OK\"}], \"is_multi_layers\": false, \"multi_layers\": {}, \"third_party\": {\"Vaidio\": {\"enable\": false, \"server_ip\": \"\"}, \"Fiix\": {\"enable\": false, \"server_ip\": \"\"}}}, \"State_2\": {\"sequence_number\": 2, \"type\": \"Detection\", \"state_id\": 806105, \"tool\": \"InstanceSeg\", \"model\": {\"model_id\": 164, \"name\": \"0413\", \"help\": [{\"True\": \"OK\", \"False\": \"NG\"}], \"class_name\": [{\"index\": 0, \"name\": \"1_line_ok\", \"color\": \"#0dd235\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 1, \"name\": \"2_EMC_push\", \"color\": \"#ea0514\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 2, \"name\": \"2_EMC_pull\", \"color\": \"#04c958\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 3, \"name\": \"3_Local\", \"color\": \"#0f9a32\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 4, \"name\": \"3_Auto\", \"color\": \"#ea2432\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 5, \"name\": \"4_Purge_on\", \"color\": \"#0fce65\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 6, \"name\": \"4_Purge_off\", \"color\": \"#e00643\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 7, \"name\": \"5_Run\", \"color\": \"#0cc756\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 8, \"name\": \"5_Stop\", \"color\": \"#de0e23\", \"price\": \"\", \"quantity\": \"\"}], \"mode\": \"validation\", \"config\": [{\"show_bbox\": true, \"show_kpts\": false, \"show_mask\": true}]}, \"is_rule\": true, \"show_doc_first\": false, \"documents\": [229], \"document_path\": [\"models/InstanceSeg_0413/slides/InstanceSeg_0413_1.png\"], \"rules\": [{\"order\": 0, \"title\": \"If (CLASS_2_EMC_push == 0 and CLASS_2_EMC_pull == 1 )_AS_OK\"}], \"is_multi_layers\": false, \"multi_layers\": {}, \"third_party\": {\"Vaidio\": {\"enable\": false, \"server_ip\": \"\"}, \"Fiix\": {\"enable\": false, \"server_ip\": \"\"}}}, \"State_3\": {\"sequence_number\": 3, \"type\": \"Detection\", \"state_id\": 108662, \"tool\": \"InstanceSeg\", \"model\": {\"model_id\": 164, \"name\": \"0413\", \"help\": [{\"True\": \"OK\", \"False\": \"NG\"}], \"class_name\": [{\"index\": 0, \"name\": \"1_line_ok\", \"color\": \"#0dd235\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 1, \"name\": \"2_EMC_push\", \"color\": \"#ea0514\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 2, \"name\": \"2_EMC_pull\", \"color\": \"#04c958\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 3, \"name\": \"3_Local\", \"color\": \"#0f9a32\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 4, \"name\": \"3_Auto\", \"color\": \"#ea2432\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 5, \"name\": \"4_Purge_on\", \"color\": \"#0fce65\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 6, \"name\": \"4_Purge_off\", \"color\": \"#e00643\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 7, \"name\": \"5_Run\", \"color\": \"#0cc756\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 8, \"name\": \"5_Stop\", \"color\": \"#de0e23\", \"price\": \"\", \"quantity\": \"\"}], \"mode\": \"validation\", \"config\": [{\"show_bbox\": true, \"show_kpts\": false, \"show_mask\": true}]}, \"is_rule\": true, \"show_doc_first\": false, \"documents\": [229], \"document_path\": [\"models/InstanceSeg_0413/slides/InstanceSeg_0413_1.png\"], \"rules\": [{\"order\": 0, \"title\": \"If (CLASS_3_Local == 1 and CLASS_3_Auto == 0 )_AS_OK\"}], \"is_multi_layers\": false, \"multi_layers\": {}, \"third_party\": {\"Vaidio\": {\"enable\": false, \"server_ip\": \"\"}, \"Fiix\": {\"enable\": false, \"server_ip\": \"\"}}}, \"State_4\": {\"sequence_number\": 4, \"type\": \"Detection\", \"state_id\": 531068, \"tool\": \"InstanceSeg\", \"model\": {\"model_id\": 164, \"name\": \"0413\", \"help\": [{\"True\": \"OK\", \"False\": \"NG\"}], \"class_name\": [{\"index\": 0, \"name\": \"1_line_ok\", \"color\": \"#0dd235\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 1, \"name\": \"2_EMC_push\", \"color\": \"#ea0514\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 2, \"name\": \"2_EMC_pull\", \"color\": \"#04c958\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 3, \"name\": \"3_Local\", \"color\": \"#0f9a32\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 4, \"name\": \"3_Auto\", \"color\": \"#ea2432\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 5, \"name\": \"4_Purge_on\", \"color\": \"#0fce65\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 6, \"name\": \"4_Purge_off\", \"color\": \"#e00643\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 7, \"name\": \"5_Run\", \"color\": \"#0cc756\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 8, \"name\": \"5_Stop\", \"color\": \"#de0e23\", \"price\": \"\", \"quantity\": \"\"}], \"mode\": \"validation\", \"config\": [{\"show_bbox\": true, \"show_kpts\": false, \"show_mask\": true}]}, \"is_rule\": true, \"show_doc_first\": false, \"documents\": [229], \"document_path\": [\"models/InstanceSeg_0413/slides/InstanceSeg_0413_1.png\"], \"rules\": [{\"order\": 0, \"title\": \"If (CLASS_4_Purge_on == 1 and CLASS_4_Purge_off == 0 )_AS_OK\"}], \"is_multi_layers\": false, \"multi_layers\": {}, \"third_party\": {\"Vaidio\": {\"enable\": false, \"server_ip\": \"\"}, \"Fiix\": {\"enable\": false, \"server_ip\": \"\"}}}, \"State_5\": {\"sequence_number\": 5, \"type\": \"Detection\", \"state_id\": 671707, \"tool\": \"InstanceSeg\", \"model\": {\"model_id\": 164, \"name\": \"0413\", \"help\": [{\"True\": \"OK\", \"False\": \"NG\"}], \"class_name\": [{\"index\": 0, \"name\": \"1_line_ok\", \"color\": \"#0dd235\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 1, \"name\": \"2_EMC_push\", \"color\": \"#ea0514\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 2, \"name\": \"2_EMC_pull\", \"color\": \"#04c958\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 3, \"name\": \"3_Local\", \"color\": \"#0f9a32\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 4, \"name\": \"3_Auto\", \"color\": \"#ea2432\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 5, \"name\": \"4_Purge_on\", \"color\": \"#0fce65\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 6, \"name\": \"4_Purge_off\", \"color\": \"#e00643\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 7, \"name\": \"5_Run\", \"color\": \"#0cc756\", \"price\": \"\", \"quantity\": \"\"}, {\"index\": 8, \"name\": \"5_Stop\", \"color\": \"#de0e23\", \"price\": \"\", \"quantity\": \"\"}], \"mode\": \"validation\", \"config\": [{\"show_bbox\": true, \"show_kpts\": false, \"show_mask\": true}]}, \"is_rule\": true, \"show_doc_first\": false, \"documents\": [229], \"document_path\": [\"models/InstanceSeg_0413/slides/InstanceSeg_0413_1.png\"], \"rules\": [{\"order\": 0, \"title\": \"If (CLASS_5_Run == 1 and CLASS_5_Stop == 0 )_AS_OK\"}], \"is_multi_layers\": false, \"multi_layers\": {}, \"third_party\": {\"Vaidio\": {\"enable\": false, \"server_ip\": \"\"}, \"Fiix\": {\"enable\": false, \"server_ip\": \"\"}}}}}";
        #else
            jsonStrin = File.ReadAllText(filePath);
        #endif
        logDisplay = jsonStrin;
        projectData = JsonConvert.DeserializeObject<ProjectData>(jsonStrin);
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


[System.Serializable]
public class ProjectData
{
    public string project_name;
    public int project_id;
    public string project_date;
    public ProjectRule project_rule;
}

[System.Serializable]
public class ProjectRule
{
    public State State_1;
    public State State_2;
    public State State_3;
    public State State_4;
    public State State_5;
}

[System.Serializable]
public class State
{
    public int sequence_number;
    public string type;
    public int state_id;
    public string tool;
    public ModelP model;
    public bool is_rule;
    public bool show_doc_first;
    public int[] documents;
    public string[] document_path;
    public Rule[] rules;
    public bool is_multi_layers;
    public MultiLayer multi_layers;
    public ThirdParty third_party;
}

[System.Serializable]
public class ModelP
{
    public int model_id;
    public string name;
    public Help[] help;
    public ClassName[] class_name;
    public string mode;
    public ConfigP[] config;
}

[System.Serializable]
public class Help
{
    public string True;
    public string False;
}

[System.Serializable]
public class ClassName
{
    public int index;
    public string name;
    public string color;
    public string price;
    public string quantity;
}

[System.Serializable]
public class ConfigP
{
    public bool show_bbox;
    public bool show_kpts;
    public bool show_mask;
}

[System.Serializable]
public class Rule
{
    public int order;
    public string title;
}

[System.Serializable]
public class MultiLayer
{
}

[System.Serializable]
public class ThirdParty
{
    public Vaidio Vaidio;
    public Fiix Fiix;
}

[System.Serializable]
public class Vaidio
{
    public bool enable;
    public string server_ip;
}

[System.Serializable]
public class Fiix
{
    public bool enable;
    public string server_ip;
}
