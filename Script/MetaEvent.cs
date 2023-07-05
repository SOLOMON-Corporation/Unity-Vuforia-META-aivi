// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Security.Cryptography;
// using System.Net;
// using UnityEngine.Networking;
// using System.Threading.Tasks;
// using System.Net.Http;
// using System.IO;
// using System.Security.Cryptography.X509Certificates;
// using System.Net.Security;
// using System.Threading;


// public class MetaEvent : MonoBehaviour
// {
//     public static event EventHandler<OnInferenceResponseEventArgs> OnInferenceResponse;
//     public class OnInferenceResponseEventArgs: EventArgs{
//         public string inferenceResponse;
//     }

//     private ARCameraScript arcamera;
//     private string responseContent;

//     void Start()
//     {
//         EventManager.OnRequestInference += Call_IEnumerator;
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     private void Call_IEnumerator(object sender, EventManager.OnRequestInferenceEventArgs e){

//         StartCoroutine(InferenceAPI(e.inferenceData.EncodeToJPG()));
//     }

//     public static void InferenceAPI(Texture2D texture2d )
//     {
//         try{
//             byte[] imageBytes = texture2d.EncodeToPNG();
//             // Stopwatch stopwatch = new Stopwatch();
//             // stopwatch.Start();
//             // string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
//             // Debug.Log("START MetAPI/InferenceAPI " + timestamp);
//             int modelID = metaResponseData.data[StationStageIndex.stageIndex].model.model_id;
//             string toolName = metaResponseData.data[StationStageIndex.stageIndex].tool;
//             int stageIDInference = metaResponseData.data[StationStageIndex.stageIndex].state_id;
//             int jobID = triggerAPIresponseData.data.job_id;
//             string apiUrl = $"https://{query_ip}/sol_server/inference";
//             // Create an HttpClientHandler object and set to use default credentials
//             HttpClientHandler handler = new HttpClientHandler();

//             var client = new HttpClient();
//             var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

//             request.Headers.Add("Model-Id", modelID.ToString());
//             request.Headers.Add("Tool-Name", toolName);
//             request.Headers.Add("State-Id", stageIDInference.ToString());
//             request.Headers.Add("Job-Id", jobID.ToString());
//             request.Headers.Add("split_text", "0");
//             request.Headers.Add("threshold", "0.1");
//             // Send the request and wait for the response

//             var content = new MultipartFormDataContent();
//             // content.Add(new StreamContent(File.OpenRead(@"C:\Users\kai_nguyen\Pictures\PF400_img - Copy.jpg")), "files", "17776490212250 - Copy.jpg");

//             Stream stream = new MemoryStream(imageBytes);
//             content.Add(new StreamContent(stream), "files", "17776490212250 - Copy.jpg");

//             // content.Add(new StreamContent(File.OpenRead(@"C:\Users\kai_nguyen\Pictures\PF400_img.jpg")), "files", "17776490212250 - Copy.jpg");

//             // Stream stream = new MemoryStream(imageBytes);
//             // content.Add(new StreamContent(stream), "image.jpg", "image/jpg");
//             request.Content = content;
//             var response =  client.SendAsync(request).Result;
//             response.EnsureSuccessStatusCode();
//             responseContent =  response.Content.ReadAsStringAsync().Result;
//             // Debug.Log(responseContent);
//             // Console.WriteLine($"Read {responseBody.Length} characters");

//             // // Check for errors
//             // if (request.result != UnityWebRequest.Result.Success)
//             // {
//             //     UnityEngine.Debug.LogError("UnityWebRequest error: " + request.error);
//             //     OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs{
//             //         inferenceResponse = ""
//             //     });
//             // }
//             // else
//             // {
//             //     // Get the response content
//             //     // responseContent = request.downloadHandler.text;
//             //     //Debug.Log(timestamp + responseContent);
//             if (responseContent.Contains("result"))
//             {
//                 responseContent = responseContent.Replace("result", "requestResult");
//             }
//             OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs
//             {
//                 inferenceResponse = responseContent
//             });
//             //     //SpinWait.SpinUntil(()=>false,1000);
//             //     isNeedToUseInference = true;
//             //         // Debug.Log(responseContent);
//             //         // Raise event here
//             // }
//             // request.Dispose = true;
//             // request.disposeDownloadHandlerOnDispose = true;

//             // form = null;
//             // client.Dispose(true);
//             // stopwatch.Stop();
//             // float elapsedTimeMs = (float)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000f;
//             // UnityEngine.Debug.Log(elapsedTimeMs);
//         }
//         catch{
//             OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs
//             {
//                 inferenceResponse = ""
//             });
//             UnityEngine.Debug.LogError("UnityWebRequest error: ");
//         }
//     }
//     public class CertificateVS : CertificateHandler
//     {
//         protected override bool ValidateCertificate(byte[] certificateData)
//         {
//             return true;
//         }
//     }
// }
