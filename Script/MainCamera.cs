using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
// using Newtonsoft.Json;
// using Pyzbar;

// public class MainCamera : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     // private static string ReadQRCode(string filePath)
//     // {
//     //     try
//     //     {
//     //         Image image = Image.FromFile(filePath);
//     //         BarcodeReader barcodeReader = new BarcodeReader();
//     //         var result = barcodeReader.Decode(image as Bitmap);
//     //         var bytesData = result.RawBytes;
//     //         var privateKey = RSA.Create();
//     //         var privateKeyPath = @"C:\Users\kai_nguyen\Pictures\private\private.pem";
//     //         var privateKeyText = File.ReadAllText(privateKeyPath);
//     //         privateKey.ImportFromPem(privateKeyText);
//     //         var jsonString = Decrypt(Convert.FromBase64String(bytesData), privateKey);
//     //         var jsonData = JsonConvert.DeserializeObject(jsonString);

//     //         Console.WriteLine(jsonData);
//     //         return jsonData;
//     //     }
//     //     catch (Exception ex)
//     //     {
//     //         Console.WriteLine(ex.Message);
//     //         return null;
//     //     }
//     // }

//     private static string Decrypt(byte[] ciphertext, RSA key)
//     {
//         try
//         {
//             var decryptedBytes = key.Decrypt(ciphertext, RSAEncryptionPadding.Pkcs1);
//             return System.Text.Encoding.UTF8.GetString(decryptedBytes);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine(ex.Message);
//             return null;
//         }
//     }


//     public class User
//     {
//         public User(Dictionary<string, object> input)
//         {
//             foreach (KeyValuePair<string, object> item in input)
//             {
//                 this.GetType().GetProperty(item.Key).SetValue(this, item.Value);
//             }
//         }
//     }

//     public static class APIHandler
//     {
//         private static int project_id = 1677721204;
//         private static string query_ip = "10.1.30.18:5000";
//         private static string image_path = @"C:\Users\kai_nguyen\Downloads\17777047773752.jpg";

//         public static string SendRequestToMeta(int projectID)
//         {
//             string url = $"https://{query_ip}/sol_server/project/{projectID}";
//             ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
//             using (var client = new WebClient())
//             {
//                 string response = client.DownloadString(url);
//                 Console.WriteLine(response);
//                 return response;
//             }
//         }

//         public static string TriggerAPI(int projectID, int stageID)
//         {
//             string url = $"https://{query_ip}/sol_server/job/trigger";
//             var myobj = new
//             {
//                 username = "admin",
//                 project_id = projectID,
//                 state_id = stageID
//             };
//             ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
//             using (var client = new WebClient())
//             {
//                 client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
//                 string json = JsonConvert.SerializeObject(myobj);
//                 string response = client.UploadString(url, json);
//                 Console.WriteLine(response);
//                 return response;
//             }
//         }

//         public static string InferenceAPI(int modelID, string toolName, int stageIDInference, int jobID)
//         {
//             string url = $"https://{query_ip}/sol_server/inference";
//             var myobj = new
//             {
//                 Model_Id = modelID.ToString(),
//                 Tool_Name = toolName,
//                 State_Id = stageIDInference.ToString(),
//                 Job_Id = jobID.ToString(),
//                 split_text = "0",
//                 threshold = "0.1"
//             };
//             ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
//             using (var client = new WebClient())
//             {
//                 client.Headers.Add("Model-Id", myobj.Model_Id);
//                 client.Headers.Add("Tool-Name", myobj.Tool_Name);
//                 client.Headers.Add("State-Id", myobj.State_Id);
//                 client.Headers.Add("Job-Id", myobj.Job_Id);
//                 client.Headers.Add("split_text", myobj.split_text);
//                 client.Headers.Add("threshold", myobj.threshold);
//                 byte[] responseBytes = client.UploadFile(url, image_path);
//                 string response = System.Text.Encoding.UTF8.GetString(responseBytes);
//                 Console.WriteLine(response);
//                 return response;
//             }
//         }

//     }
// }
