using System;
using System.Collections;
using UnityEngine;
using System.Net;
using UnityEngine.Networking;
using static JsonDeserialization;

public static class MetaService
{
    public static event EventHandler<OnInferenceResponseEventArgs> OnInferenceResponse;
    public class OnInferenceResponseEventArgs: EventArgs{
        public string inferenceResponse;
    }
    public static string serverIP;
    public static string[] qrMetaData;
    public static int imageWidth2Meta = 640;
    public static int imageHeight2Meta = 480;

    // API end points
    private static string projectApiEndPoint = "sol_server/project";
    private static string stageApiEndPoint = "sol_server/job/trigger";
    private static string inferenceApiEndPoint = "sol_server/inference";
    public static StageData stageData{get; private set;}
    public static ProjectData projectData {get; private set;}
    

    /// <summary>
    /// Connects with the Meta project using the specified project ID.
    /// </summary>
    /// <returns>The project data received from the API.</returns>
    public static void ConnectWithMetaProjectID()
    {
        int projectID = Int32.Parse(MetaService.qrMetaData[3]);
        var metaResponse = RequestProjectAPI(projectID);
        PrepareResponseForDeserialization(metaResponse);
        projectData = JsonUtility.FromJson<ProjectData>(metaResponse);// ?? new ResponseData();
        if (!projectData.requestResult){
            return;
        }
    }

    /// <summary>
    /// Connects with Meta stage ID and retrieves the stage data.
    /// </summary>
    /// <returns>The stage data.</returns>
    public static void ConnectWithMetaStageID(){
        try{
            int projectID = Int32.Parse(MetaService.qrMetaData[3]);
            int stageID = projectData.data[StationStageIndex.stageIndex - 1].state_id;
            // Request stage API
            var triggerAPIresponse = RequestStageAPI(projectID, stageID);

            // Prepare response for deserialization
            triggerAPIresponse = PrepareResponseForDeserialization(triggerAPIresponse);

            // Deserialize response data
            stageData = JsonUtility.FromJson<StageData>(triggerAPIresponse);

        }
        catch
        {
            // Handle error case
            stageData = JsonUtility.FromJson<StageData>("{\"data\":{},\"requestResult\":false}");
        }
    }
    /// <summary>
    /// Sends a GET request to the project API endpoint to retrieve project data.
    /// </summary>
    /// <param name="projectID">The ID of the project.</param>
    /// <returns>The response received from the server.</returns>
    public static string RequestProjectAPI(int projectID)
    {
        string url = $"https://{serverIP}/{projectApiEndPoint}/{projectID}";

        // Bypass SSL certificate validation
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        using (var client = new WebClient())
        {
            // Send GET request and receive response
            string response = client.DownloadString(url);
            return response;
        }
    }
    /// <summary>
    /// Sends a POST request to the stage API endpoint with the provided project and stage IDs.
    /// </summary>
    /// <param name="projectID">The ID of the project.</param>
    /// <param name="stageID">The ID of the stage.</param>
    /// <returns>The response string received from the server.</returns>
    public static string RequestStageAPI(int projectID, int stageID)
    {
        string url = $"https://{serverIP}/{stageApiEndPoint}";

        // Create POST data object
        TriggerPostData sendData = new TriggerPostData
        {
            username = "admin",
            project_id = projectID,
            state_id = stageID
        };

        // Bypass SSL certificate validation
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        using (var client = new WebClient())
        {
            // Set content type and serialize POST data object to JSON
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string json = JsonUtility.ToJson(sendData);

            // Send POST request and receive response
            string response = client.UploadString(url, json);
            return response;
        }
    }
    //AI Inference
    public static IEnumerator InferenceAPI(byte[] imageBytes )
    {
        int modelID = projectData.data[StationStageIndex.stageIndex-1].model.model_id;
        string toolName = projectData.data[StationStageIndex.stageIndex-1].tool;
        int stageIDInference = projectData.data[StationStageIndex.stageIndex-1].state_id;
        int jobID = stageData.data.job_id;
        string apiUrl = $"https://{serverIP}/{inferenceApiEndPoint}";

        // Create form data
        WWWForm form = new WWWForm();
        form.AddBinaryData("files", imageBytes, "image.jpg", "image/jpg");

        // Bypass SSL certificate validation
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, form))
        {
            // Set request headers
            request.SetRequestHeader("Model-Id", modelID.ToString());
            request.SetRequestHeader("Tool-Name", toolName);
            request.SetRequestHeader("State-Id", stageIDInference.ToString());
            request.SetRequestHeader("Job-Id", jobID.ToString());
            request.SetRequestHeader("split_text", "0");
            request.SetRequestHeader("threshold", "0.1");
            request.certificateHandler = new CertificateVS();

            // Send request and wait for response
            yield return request.SendWebRequest();

            string responseContent = "";

            // Check for successful response
            if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError)
            {
                try
                {
                    responseContent = request.downloadHandler.text;
                }
                catch
                {
                    responseContent = "";
                }
            }

            // Prepare response for deserialization
            responseContent = PrepareResponseForDeserialization(responseContent);

            // Invoke inference response event
            OnInferenceResponse?.Invoke(null, new OnInferenceResponseEventArgs
            {
                inferenceResponse = responseContent
            });

            // Dispose handlers
            request.disposeUploadHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;
        }

        form = null;

        yield return null;
    }

    /// <summary>
    /// Prepares the response string for deserialization by replacing "result" with "requestResult" to match the data class.
    /// </summary>
    /// <param name="metaResponse">The original response string</param>
    /// <returns>The modified response string with "result" replaced by "requestResult"</returns>
    private static string PrepareResponseForDeserialization(string metaResponse)
    {
        try
        {
            // Replace "result" with "requestResult" to match data class
            return metaResponse.Replace("result", "requestResult") ?? string.Empty;
        }
        catch
        {
            return metaResponse;
        }
    }

    // Custom Certificate Validation Handler used for Unity's Certificate Validation
    public class CertificateVS : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}