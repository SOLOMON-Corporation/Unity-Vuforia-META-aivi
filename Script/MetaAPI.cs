using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Net;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

public class MetaAPI : MonoBehaviour
{
    // public string inferenceResponseContent;
    public event EventHandler<OnInferenceResponseEventArgs> OnInferenceResponse;
    public class OnInferenceResponseEventArgs: EventArgs{
        public string inferenceResponse;
    }
    public bool isInferenceRunning {get; private set ;}
    public string query_ip;// = "10.1.30.18:5000";
    
    public RegisterResponse triggerAPIresponseData{get; private set;}
    public ResponseData metaResponseData{get; private set;}
    private string responseContent;
    [SerializeField]  public TMPro.TextMeshProUGUI metaTextMessage;
    // Start is called before the first frame update
    public void ConnectMetaBasedProjectID(int project_id)
    {
        // Debug.Log("START ConnectMetaBasedProjectID");
        // Send Request To Meta to get information
        var metaResponse = SendRequestToMeta(project_id);
        if (metaResponse.Contains("result")){
            metaResponse = metaResponse.Replace("result", "requestResult");
        }
        // Debug.Log(metaResponse);
        metaResponseData = JsonUtility.FromJson<ResponseData>(metaResponse);// ?? new ResponseData();
        // Debug.Log(metaResponseData.requestResult);
        if (!metaResponseData.requestResult){
            Debug.Log("SendRequestToMeta Fail!");
            metaTextMessage.text = "SendRequestToMeta Fail!";
            return;
        }

        // TriggerAPI to get job_id
        var triggerAPIresponse = TriggerAPI(project_id, metaResponseData.data[0].state_id);
        if (triggerAPIresponse.Contains("result")){
            triggerAPIresponse = triggerAPIresponse.Replace("result", "requestResult");
        }
        // Debug.Log(triggerAPIresponse);
        triggerAPIresponseData = JsonUtility.FromJson<RegisterResponse>(triggerAPIresponse);
        if (!triggerAPIresponseData.requestResult){
            metaTextMessage.text = "TriggerAPI Fail!";
        }
        // Temp: Loop for inference
        // StartCoroutine(InferenceAPI(File.ReadAllBytes(image_path)));
        // Debug.Log("Finish");
        // Debug.Log("END ConnectMetaBasedProjectID");
    }

    public string SendRequestToMeta(int projectID)
    {
        // Debug.Log("START SendRequestToMeta");
        string url = $"https://{query_ip}/sol_server/project/{projectID}";
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        using (var client = new WebClient())
        {
            string response = client.DownloadString(url);
            //Console.WriteLine(response);
            return response;
        }
        // Debug.Log("END SendRequestToMeta");
    }

    public string TriggerAPI(int projectID, int stageID)
    {
        string url = $"https://{query_ip}/sol_server/job/trigger";
        TriggerPostData sendData = new TriggerPostData
        {
            username = "admin",
            project_id = projectID,
            state_id = stageID
        };
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string json = JsonUtility.ToJson(sendData);//JsonConvert.SerializeObject(myobj);
            string response = client.UploadString(url, json);
            //Console.WriteLine(response);
            return response;
        }
    }

    public IEnumerator InferenceAPI(byte[] imageBytes )
    {
        isInferenceRunning = true;
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        // Debug.Log("START MetAPI/InferenceAPI " + timestamp);
        int modelID = metaResponseData.data[1].model.model_id;
        string toolName = metaResponseData.data[1].tool;
        int stageIDInference = metaResponseData.data[1].state_id;
        int jobID = triggerAPIresponseData.data.job_id;
        // Specify the URL of the API endpoint to request
        string apiUrl = $"https://{query_ip}/sol_server/inference";

        WWWForm form = new WWWForm();
        //Add the image data to the form data
        form.AddBinaryData("files", imageBytes, "image.jpg", "image/jpg");

        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //Send the form data as a POST request to the server
        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, form))
        {
           request.SetRequestHeader("Model-Id", modelID.ToString());
           request.SetRequestHeader("Tool-Name", toolName);
           request.SetRequestHeader("State-Id", stageIDInference.ToString());
           request.SetRequestHeader("Job-Id", jobID.ToString());
           request.SetRequestHeader("split_text", "0");
           request.SetRequestHeader("threshold", "0.1");
           request.certificateHandler = new CertificateVS();
           // Send the request and wait for the response
           yield return request.SendWebRequest();

           // Check for errors
           if (request.result != UnityWebRequest.Result.Success)
           {
               Debug.LogError(request.error);
           }
           else
           {
               // Get the response content
               responseContent = request.downloadHandler.text;
               //Debug.Log(timestamp + responseContent);
               if (responseContent.Contains("result")){
                   responseContent = responseContent.Replace("result", "requestResult");
               }
               isInferenceRunning = false;
               OnInferenceResponse?.Invoke(this, new OnInferenceResponseEventArgs{
                   inferenceResponse = responseContent
               });
               // Debug.Log(responseContent);
               // Raise event here
           }
           request.disposeUploadHandlerOnDispose = true;
           request.disposeDownloadHandlerOnDispose = true;
        }
        form = null;
        // Debug.Log("END MetAPI/InferenceAPI " + timestamp);
        yield return null;
        yield break;
    }

    //Validation Certificate for VS
    public class CertificateVS : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}


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
public class RegisterResponse
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
public class ResponseData
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