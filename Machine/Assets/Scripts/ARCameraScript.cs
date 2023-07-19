using System;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Collections.Generic;
using static JsonDeserialization;

public class ARCameraScript : MonoBehaviour
{
    [SerializeField] private Texture2D greenBBox;
    [SerializeField] private Texture2D redBBox;
    [SerializeField] private Camera arCamera;
    [SerializeField] private RawImage resultStageDispalayImage;
    [SerializeField] private Button nextStepBtn;
    [SerializeField] private GameObject captureBtn;
    [SerializeField] private int bBoxBorderSize = -1;
    [SerializeField]  private TMPro.TextMeshProUGUI titleInfo;
    [SerializeField]  private TMPro.TextMeshProUGUI logInfo;
    private GameObject bBoxGameobject;
    private Texture2D capturedTexture;
    private List<Datastage> dataStages;
    private GameObject checkListGameObject;
    private Transform checkMarkTransform;
    private RenderTexture renderTexture;
    private Rect bBoxRect;
    private GUIStyle guiStyle = new GUIStyle();
    private Coroutine coroutineControler;
    private int reconnectedCount = 0;
    private byte[] CapturedImage;
    public static bool inferenceResponseFlag = true;
    private void Start()
    {
        // Set the StationStageIndex FunctionIndex to "Home"
        StationStageIndex.FunctionIndex = "Home";

        // Subscribe to the OnInferenceResponse event in MetaApiStatic
        MetaService.OnInferenceResponse += OnInferenceResponse;

        //EventManager.OnStageChange += OnUpdate3DModelName;

        // Create a new RenderTexture with specified dimensions
        renderTexture = new RenderTexture(MetaService.imageWidth2Meta, MetaService.imageHeight2Meta, 24);

        // Create a new Texture2D with specified dimensions and format
        capturedTexture = new Texture2D(MetaService.imageWidth2Meta, MetaService.imageHeight2Meta, TextureFormat.RGB24, false);

        // Set the size of the resultStageDispalayImage's RectTransform to match the screen size
        resultStageDispalayImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        // Set the border of the GUI style with a specified size
        guiStyle.border = new RectOffset(bBoxBorderSize, bBoxBorderSize, bBoxBorderSize, bBoxBorderSize);
    }

    private void OnUpdate3DModelName(object sender, EventManager.OnStageIndexEventArgs e)
    {
        bBoxGameobject = GameObject.Find(e.stageName);
    }

        private void OnInferenceResponse(object sender, MetaService.OnInferenceResponseEventArgs e)
    {
        // Listen event from Inference response in Meta API, event parameter will be drawn in this function
        Debug.Log("START ARCameraScript/OnInferenceResponse ");

        // Check if the function index is not "Detect"
        if (StationStageIndex.FunctionIndex != "Detect")
        {
            return;
        }

        try
        {
            // Deserialize the inference response
            InferenceResult metaAPIinferenceData = JsonConvert.DeserializeObject<InferenceResult>(e.inferenceResponse);

            // Check if rule data is not null
            if (!metaAPIinferenceData.data.rule.Equals(null))
            {
                // Set Detection result
                if (!StationStageIndex.metaInferenceRule && metaAPIinferenceData.data.rule)
                {
                    StationStageIndex.metaInferenceRule = metaAPIinferenceData.data.rule;
                    dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;

                    // Show/hide next step and capture buttons based on current stage
                    if (StationStageIndex.stageIndex < dataStages.Count - 1)
                    {
                        nextStepBtn.gameObject.SetActive(true);
                        captureBtn.gameObject.SetActive(false);
                    }

                    // Stop the metaTimeCount if it's not null
                    if (StationStageIndex.metaTimeCount != null)
                    {
                        StationStageIndex.metaTimeCount.Stop();
                    }

                    // Update bounding box
                    //if (bBoxGameobject != null)
                    //{
                    //    if (metaAPIinferenceData.data.rois != null)
                    //    {
                    //        int[] rois = metaAPIinferenceData.data.rois[0];
                    //        Vector3 centerPoint; float radiusOnScreen;
                    //        (centerPoint, radiusOnScreen) = GetObjectCenterRadius();

                    //        Vector3 roisMeta = new Vector3((rois[2] + rois[0]) * Screen.width / MetaService.imageWidth2Meta / 2,
                    //                                Screen.height - (rois[3] + rois[1]) * Screen.height / MetaService.imageHeight2Meta / 2,
                    //                                centerPoint.z);
                    //        if (Vector2.Distance(centerPoint, roisMeta) > radiusOnScreen / 4)
                    //        {
                    //            bBoxGameobject.transform.position = arCamera.ScreenToWorldPoint(new Vector3(roisMeta.x, Screen.height - roisMeta.y, roisMeta.z));
                    //        }
                    //    }
                    //}

                    // Find the checkListGameObject and show the checkmark
                    checkListGameObject = GameObject.Find("CP" + StationStageIndex.stageIndex.ToString());
                    checkMarkTransform = checkListGameObject.transform.Find("Background").transform.Find("Checkmark");
                    checkMarkTransform.gameObject.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR ARCameraScript/OnInferenceResponse" + ex.ToString());
        }

        // Clean up and set inferenceResponseFlag to true
        e = null;
        inferenceResponseFlag = true;

        Debug.Log("END ARCameraScript/OnInferenceResponse ");
    }

    public (Vector3, float) GetObjectCenterRadius()
    {
        // Get checkpoint position and scale
        Vector3 thisCheckpointPosition = StationStageIndex.gameObjectPoints[$"{StationStageIndex.stageIndex}"][0];
        Vector3 thisCheckpointScale = StationStageIndex.gameObjectPoints[$"{StationStageIndex.stageIndex}"][1];

        // Calculate normal vectors and line direction
        Vector3 normalVector1 = GeometryUtils.CalculateNormal(thisCheckpointPosition,
                        arCamera.transform.position,
                        thisCheckpointPosition + new Vector3(0f, thisCheckpointScale.x / 2, 0f));
        Vector3 normalVector2 = arCamera.transform.position - thisCheckpointPosition;
        Vector3 lineDirection = Vector3.Cross(normalVector1, normalVector2).normalized;

        // Calculate screen positions
        Vector3 topPosition = GetScreenSpacePoint(thisCheckpointPosition + lineDirection * thisCheckpointScale.y / 2);
        Vector3 centerPoint = GetScreenSpacePoint(thisCheckpointPosition);

        // Calculate radius on screen and bounding box position
        float radiusOnScreen = Vector2.Distance(topPosition, centerPoint);
        return (centerPoint, radiusOnScreen);
    }

    public Rect GetObjectBBox()
    {
        Vector3 centerPoint; float radiusOnScreen;
        (centerPoint, radiusOnScreen) = GetObjectCenterRadius();
        Vector3 corner1World2D = centerPoint - new Vector3(radiusOnScreen, radiusOnScreen, 0f);
        Rect unityRect = new Rect((int)(corner1World2D[0]), (int)(corner1World2D[1]), radiusOnScreen * 2, radiusOnScreen * 2);
        return unityRect;
    }

    public void DrawRois(bool drawOnResultStage)
    {

        if (!drawOnResultStage)
        {
            // Update bounding box position
            bBoxRect = GetObjectBBox();
        }

        // Set GUI style and label based on meta inference rule
        if (StationStageIndex.metaInferenceRule)
        {
            guiStyle.normal.background = greenBBox;
        }
        else
        {
            guiStyle.normal.background = redBBox;
        }

        // Draw bounding box
        GUI.Box(bBoxRect, "", guiStyle);

    }

    void OnGUI()
    {
        if (StationStageIndex.FunctionIndex == "Detect"){
            DrawRois(false);
        }
        else if (StationStageIndex.FunctionIndex == "Result"){
            DrawRois(true);
        }
    }

    private void Update()
    {
        // Check if the current function index is "Detect"
        if (StationStageIndex.FunctionIndex == "Detect")
        {
            // Check if triggerAPIresponseData.result is null or false then reconnect
            if (MetaService.stageData == null || !MetaService.stageData.requestResult)
            {
                logInfo.text = "Reconnect triggerAPIresponseData" + MetaService.stageData;
                reconnectedCount++;

                // If reconnectedCount exceeds 100, reset it and connect to Meta based on project ID
                if (reconnectedCount > 100)
                {
                    reconnectedCount = 0;
                    MetaService.ConnectWithMetaProjectID();
                }

                // Connect to Meta based on stage
                MetaService.ConnectWithMetaStageID();
                return;
            }

            // If inferenceResponseFlag is true, perform necessary actions
            if (inferenceResponseFlag)
            {
                inferenceResponseFlag = false;
                SendImage2Meta();
            }

            // Set the titleInfo text to display elapsed minutes and seconds from metaTimeCount
            titleInfo.text = $"{StationStageIndex.metaTimeCount.Elapsed.Minutes}:{StationStageIndex.metaTimeCount.Elapsed.Seconds}";// +  " " + usedMemory / 1024000 + " MB";
        }
        else
        {
            // Reset titleInfo text if the function index is not "Detect"
            titleInfo.text = "";
        }
    }

    private void SendImage2Meta()
    {
        // Set the target texture of the AR camera to the render texture
        arCamera.targetTexture = renderTexture;

        // Render the AR camera
        arCamera.Render();

        // Set the active render texture
        RenderTexture.active = renderTexture;

        // Read the pixels from the specified rectangle in the capture texture
        capturedTexture.ReadPixels(new Rect(0, 0, MetaService.imageWidth2Meta, MetaService.imageHeight2Meta), 0, 0);

        // Apply the changes made to the capture texture
        capturedTexture.Apply();

        // Encode the capture texture as JPG and assign it to the CapturedImage variable
        CapturedImage = capturedTexture.EncodeToJPG();

        // Check if there is a coroutine already running and stop it
        if (coroutineControler != null)
        {
            StopCoroutine(coroutineControler);
        }

        // Start a new coroutine for the InferenceAPI using the captured image
        coroutineControler = StartCoroutine(MetaService.InferenceAPI(CapturedImage));

        // Reset the target and active render textures
        arCamera.targetTexture = null;
        RenderTexture.active = null;
    }

    // Takes a screenshot and displays it on the result stage display image
    public void TakeScreenshot()
    {
        // Assign the captured texture to the result stage display image texture
        resultStageDispalayImage.texture = capturedTexture;
    }

    private Vector3 GetScreenSpacePoint(Vector3 worldPosition)
    {
        // Convert the world position to screen space point
        Vector3 screenSpacePoint = arCamera.WorldToScreenPoint(worldPosition);

        // Invert the y-coordinate to match the screen space (0,0) at the bottom left
        screenSpacePoint.y = Screen.height - screenSpacePoint.y;

        return screenSpacePoint;
    }
}