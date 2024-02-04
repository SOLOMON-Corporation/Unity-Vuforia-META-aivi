using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonDeserialization
{
    [System.Serializable]
    public class InferenceResult
    {
        public InferenceData data;
        public string message;
        public bool requestResult;
    }

    [System.Serializable]
    public class InferenceData
    {
        public int[] class_ids;
        public float[] keypoints;
        public List<int[]> masks_shape_list;
        public List<int[]> rois;
        public bool rule;
        public float[] scores;
        public string[] word;
        // public string masks;
    }
    [System.Serializable]
    public class StageData
    {
        public DataTrigger data;
        // public string message;
        public bool requestResult;
    }
    [System.Serializable]
    public class DataTrigger
    {
        public int job_id;
        public string time;
    }
    [System.Serializable]
    public class ProjectData
    {
        public List<DataItem> data;
        // public string message;
        public bool requestResult;
    }
    [System.Serializable]
    public class DataItem
    {
        public List<string> document_path;
        public List<int> documents;
        public bool is_multi_layers;
        public bool is_rule;
        public Model model;
        public Dictionary<string, object> multi_layers;
        public List<object> rules;
        public int sequence_number;
        public bool show_doc_first;
        public int state_id;
        public Dictionary<string, ThirdPartyData> third_party;
        public string tool;
        public string type;
    }
    [System.Serializable]
    public class Model
    {
        public List<Class> class_name;
        public List<Config> config;
        public List<object> help;
        public string mode;
        public int model_id;
        public string name;
    }
    [System.Serializable]
    public class Class
    {
        public string color;
        public int index;
        public string name;
        public string price;
        public string quantity;
    }
    [System.Serializable]
    public class Config
    {
        public bool show_bbox;
        public bool show_kpts;
        public bool show_mask;
    }

    [System.Serializable]
    public class ThirdPartyData
    {
        public bool enabled;
        public string version;
    }

    [System.Serializable]
    public class TriggerPostData
    {
        public string username;
        public int project_id;
        public int state_id;
    }
}
