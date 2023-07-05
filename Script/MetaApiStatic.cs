using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Net;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Threading;
using OpenCvSharp;

public static class MetaApiStatic
{
    public static string inferenceResponseContent;
    public static event EventHandler<OnInferenceResponseEventArgs> OnInferenceResponse;
    public class OnInferenceResponseEventArgs: EventArgs{
        public string inferenceResponse;
    }
    public static bool isNeedToUseInference = false;
    public static string query_ip;// = "10.1.30.18:5000";
    public static string[] qrMetaData;
    public static RegisterResponse triggerAPIresponseData{get; private set;}
    public static ResponseData metaResponseData{get; private set;}
    public static bool isRunning = true;
    public static byte[] imageBytes;
    // [SerializeField]  public static TMPro.TextMeshProUGUI metaTextMessage;

    // private static string ReadQRCode(string barcodeString)
    // {
    //     try
    //     {
    //         var privateKey = RSA.Create();
    //         var privateKeyPath = @"C:\Users\kai_nguyen\Pictures\private\private.pem";
    //         var privateKeyText = File.ReadAllText(privateKeyPath);
    //         privateKey.ImportFromPem(privateKeyText);
    //         var jsonString = Decryption(Convert.FromBase64String(barcodeString), privateKey);
    //         var jsonData = JsonConvert.DeserializeObject(jsonString);

    //         Console.WriteLine(jsonData);
    //         return jsonData;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return null;
    //     }
    // }
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
    public class TriggerPostData{
        public string username;
        public int project_id;
        public int state_id;
    }

    // Start is called before the first frame update
    public static void ConnectMetaBasedProjectID()
    {
        // Debug.Log("START ConnectMetaBasedProjectID");
        // Send Request To Meta to get information
        var metaResponse = SendRequestToMeta(Int32.Parse(MetaApiStatic.qrMetaData[3]));
        if (metaResponse.Contains("result")){
            metaResponse = metaResponse.Replace("result", "requestResult");// Unity can not use result as variable
        }
        // Debug.Log(metaResponse);
        metaResponseData = JsonUtility.FromJson<ResponseData>(metaResponse);// ?? new ResponseData();
        // Debug.Log(metaResponseData.requestResult);
        if (!metaResponseData.requestResult){
            // Debug.Log("SendRequestToMeta Fail!");
            // metaTextMessage.text = "SendRequestToMeta Fail!";
            return;
        }
    }
    public static void ConnectMetaBasedStage(){
        // TriggerAPI to get job_id
        var triggerAPIresponse = TriggerAPI(Int32.Parse(MetaApiStatic.qrMetaData[3]), metaResponseData.data[StationStageIndex.stageIndex-1].state_id);
        if (triggerAPIresponse.Contains("result")){
            triggerAPIresponse = triggerAPIresponse.Replace("result", "requestResult");// Unity can not use result as variable
        }
        // Debug.Log(triggerAPIresponse);
        triggerAPIresponseData = JsonUtility.FromJson<RegisterResponse>(triggerAPIresponse);
        if (!triggerAPIresponseData.requestResult){
            // metaTextMessage.text = "TriggerAPI Fail!";
        }
        // Temp: Loop for inference
        // StartCoroutine(InferenceAPI(File.ReadAllBytes(image_path)));
        // Debug.Log("Finish");
        // Debug.Log("END ConnectMetaBasedProjectID");
    }

    private static string Decryption(byte[] ciphertext, RSA key)
    {
        try
        {
            var decryptedBytes = key.Decrypt(ciphertext, RSAEncryptionPadding.Pkcs1);
            return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public static string SendRequestToMeta(int projectID)
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

    public static string TriggerAPI(int projectID, int stageID)
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

    // public static async Task InferenceAPI__(byte[] imageBytes)//Mock function
    //     {
    //         // isInferenceRunning = true;
    //         // await Task.Yield();
    //         await Task.Delay(1000);
    //         OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs{
    //                 inferenceResponse = "{\"data\":{\"class_ids\":[6,3,1],\"keypoints\":[],\"masks\":\"\",\"masks_shape_list\":[[140,210],[122,203],[341,346]],\"rois\":[[1690,321,1830,531],[1840,317,1962,520],[1272,275,1613,621]],\"rule\":true,\"scores\":[0.866072416305542,0.8237119317054749,0.6006558537483215],\"word\":[]},\"message\":\"Inference successfully\",\"requestResult\":true}\n"
    //         });
    //         isNeedToUseInference = true;
    //         // using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
    //         // {
    //         //     await Task.Yield();
    //         //     request.SendWebRequest();
    //         // }

    // }

    public static async Task InferenceAPI(byte[] imageBytes)//(byte[] imageBytes)//
    {
        try{
            string ss = "";
            string responseContent;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // await Task.Delay(3000);

            
            //Stream stream = new MemoryStream();
            //matImage.WriteToStream(stream);

            //imageBytes = texture2d.EncodeToJPG();
            stopwatch.Stop(); ss += $"EncodeToJPG: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
            int modelID = metaResponseData.data[StationStageIndex.stageIndex-1].model.model_id;
            string toolName = metaResponseData.data[StationStageIndex.stageIndex-1].tool;
            int stageIDInference = metaResponseData.data[StationStageIndex.stageIndex-1].state_id;
            int jobID = triggerAPIresponseData.data.job_id;
            string apiUrl = $"https://{query_ip}/sol_server/inference";
            // Create an HttpClientHandler object and set to use default credentials
            HttpClientHandler handler = new HttpClientHandler();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            request.Headers.Add("Model-Id", modelID.ToString());
            request.Headers.Add("Tool-Name", toolName);
            request.Headers.Add("State-Id", stageIDInference.ToString());
            request.Headers.Add("Job-Id", jobID.ToString());
            request.Headers.Add("split_text", "0");
            request.Headers.Add("threshold", "0.1");
            // Send the request and wait for the response
            stopwatch.Stop(); ss += $"Headers: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
            var content = new MultipartFormDataContent();
            // content.Add(new StreamContent(File.OpenRead(@"C:\Users\kai_nguyen\Pictures\PF400_img - Copy.jpg")), "files", "17776490212250 - Copy.jpg");
            Stream stream = new MemoryStream(imageBytes);
            content.Add(new StreamContent(stream), "files", "17776490212250 - Copy.jpg");
            request.Content = content;
            var response =  await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            responseContent = await response.Content.ReadAsStringAsync();
            stopwatch.Stop(); ss += $"await: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                // Get the response content
            // responseContent = request.downloadHandler.text;
            //Debug.Log(timestamp + responseContent);
            if (responseContent.Contains("result")){
                responseContent = responseContent.Replace("result", "requestResult");
            }
            OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs{
            inferenceResponse = responseContent
            });
            stopwatch.Stop(); ss += $"Invoke: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
        }
        catch (System.Exception ex){
            UnityEngine.Debug.Log(ex);
            OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs{
            inferenceResponse = ""
            });
        }

    }
    // public static IEnumerator InferenceAPI_(Texture2D texture2d )//(byte[] imageBytes)//
    // {
    //     // isNeedToUseInference = false;
    //     imageBytes = texture2d.EncodeToJPG();
    //     // string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
    //     // Debug.Log("START MetAPI/InferenceAPI " + timestamp);
    //     int modelID = metaResponseData.data[StationStageIndex.stageIndex].model.model_id;
    //     string toolName = metaResponseData.data[StationStageIndex.stageIndex].tool;
    //     int stageIDInference = metaResponseData.data[StationStageIndex.stageIndex].state_id;
    //     int jobID = triggerAPIresponseData.data.job_id;
    //     string apiUrl = $"https://{query_ip}/sol_server/inference";

    //     // modelID = 159;
    //     // toolName = "InstanceSeg";
    //     // stageIDInference = 292797;
    //     // jobID = 1557214;
    //     // apiUrl = $"https://125.227.130.192:5000/sol_server/inference";

    //     WWWForm form = new WWWForm();
    //     //Add the image data to the form data
    //     form.AddBinaryData("files", imageBytes, "image.jpg", "image/jpg");

    //     ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    //     //Send the form data as a POST request to the server
    //     using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, form))
    //     {
    //         request.SetRequestHeader("Model-Id", modelID.ToString());
    //         request.SetRequestHeader("Tool-Name", toolName);
    //         request.SetRequestHeader("State-Id", stageIDInference.ToString());
    //         request.SetRequestHeader("Job-Id", jobID.ToString());
    //         request.SetRequestHeader("split_text", "0");
    //         request.SetRequestHeader("threshold", "0.1");
    //         request.certificateHandler = new CertificateVS();
    //         // Send the request and wait for the response
    //         yield return request.SendWebRequest();
    //         yield return new WaitForSeconds(3);
    //         // Check for errors
    //         if (request.result != UnityWebRequest.Result.Success)
    //         {
    //             // Debug.LogError("UnityWebRequest error: " + request.error);
    //             // OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs{
    //             //     inferenceResponse = ""
    //             // });
    //         }
    //         else
    //         {
    //             // Get the response content
    //             responseContent = request.downloadHandler.text;
    //             //Debug.Log(timestamp + responseContent);
    //             if (responseContent.Contains("result")){
    //                 responseContent = responseContent.Replace("result", "requestResult");
    //             }
    //             isNeedToUseInference = true;
    //             OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs{
    //                 inferenceResponse = responseContent
    //             });
    //             // Debug.Log(responseContent);
    //             // Raise event here
    //         }
    //         request.disposeUploadHandlerOnDispose = true;
    //         request.disposeDownloadHandlerOnDispose = true;
    //     }
    //     form = null;
    //     // Debug.Log("END MetAPI/InferenceAPI " + timestamp);
    //     yield return null;
    //     yield break;
    // }


    // //Validation Certificate for VS
    // public class CertificateVS : CertificateHandler
    // {
    //     protected override bool ValidateCertificate(byte[] certificateData)
    //     {
    //         return true;
    //     }
    // }

    // private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
    // {
    //     // It is possible to inspect the certificate provided by the server.
    //     Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
    //     Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
    //     Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
    //     Console.WriteLine($"Issuer: {certificate.Issuer}");
    //     Console.WriteLine($"Subject: {certificate.Subject}");

    //     // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
    //     Console.WriteLine($"Errors: {sslErrors}");
    //     return sslErrors == SslPolicyErrors.None;
    // }

}
