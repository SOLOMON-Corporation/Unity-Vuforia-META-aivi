using System;
using UnityEngine;
using Vuforia;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Runtime.InteropServices;
using Unity.Collections;
//using static DecryptionRSA;
public class ARCameraScript : MonoBehaviour
{
    // public MetaAPI metaAPI;
    const PixelFormat PIXEL_FORMAT = PixelFormat.RGB888;
    const TextureFormat TEXTURE_FORMAT = TextureFormat.RGB24;
    private Texture2D mTexture;
    private Texture2D screenShot;
    private UnityEngine.UI.Image imageComponent;
    private RenderTexture renderTexture;
    bool mFormatRegistered = true;
    private RawImage rawImage;
    // private static int project_id = 1678930460;
    private static string image_path = @"C:\Users\kai_nguyen\Pictures\PF400_img.jpg";
    [SerializeField] private Color fillColor = new Color(0, 0, 0, 0.1f); // transparent white
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private int thickness = 100;
    [SerializeField] private TMPro.TextMeshProUGUI uiMessage;
    private InferenceResult metaAPIinferenceData;
    private int[] roisClassId;
    private int imageWidth = 640;
    private int imageHeight = 480;
    private DefaultObserverEventHandler observer;
    private byte[] screenShot_image;
    public Camera mainCamera;
    private Camera _mainCamera;
    private Texture2D copyTexture;
    List<Vector3[]> metaRoisWolrd;
    List<Vector2[]> rois2D;
    // List<Vector2[]> metaRois = new List<Vector2[]>();
    private float[] corner_distance;
    private WebCamTexture webCamTexture;
    private RectTransform bbox;
    public RawImage capture_image;
    public Texture2D GreenBoxBorder;
    public Texture2D RedBoxBorder;
    private List<UnityEngine.Rect> rectBboxes;
    public static bool inferenceResponseFlag = true;
    private Thread thread;
    // private bool imageTargetFound = false;
    private void Start()
    {
        _mainCamera = new GameObject("_mainCamera").AddComponent<Camera>();
        _mainCamera.enabled = false;
        // _mainCamera = mainCamera;
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        // EventManager.OnBarCodeDetectedEvent += OnBarCodeDetectedHandler;
        MetaApiStatic.OnInferenceResponse += OnInferenceResponse;
        screenShot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        
        capture_image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        // GameObjectController.OnStartDetection += CallCaptureScreen;
        Camera.onPostRender += OnPostRenderCallback;
        // StationStageIndex.OnFunctionIndexChange += CallCaptureScreen;
        // Debug.Log(Application.streamingAssetsPath);
        // // Get the default box skin
        // boxStyle = GUI.skin.box;
        // // Set the border color to black
        // boxStyle.normal.textColor = Color.black;
        // // Set the background color to transparent
        // boxStyle.normal.background = Texture2D.whiteTexture;
    }
    void OnDestroy()
    {
        Camera.onPostRender -= OnPostRenderCallback;
    }

    public void CallMetaAPIinference()
    {
        try
        {
            webCamTexture = new WebCamTexture();
            UnityEngine.Debug.Log("START CallMetaAPIinference");
            if (!MetaApiStatic.isNeedToUseInference)
            {
                // cameraImage = GetCameraImageLoc().EncodeToPNG();
                // string timestamp = DateTime.UtcNow.ToString("ss.fff");
                // Debug.Log(cameraImage.Length + timestamp);
                // ImageProcessing.SaveImageToAssets(cameraImage);
                // SaveImageToAssets(cameraImage);
                // byte[] cameraImage = File.ReadAllBytes(image_path);
                // if (screenShot_image == null){
                //     screenShot_image = TakeScreenshot_demo().EncodeToPNG();
                // }
                // screenShot_image = TakeScreenshot_from_savedCamera().EncodeToPNG();
                //hungnc start
                // screenShot_image = TakeScreenshot_demo().EncodeToPNG();
                // // ImageProcessing.SaveImageToAssets(screenShot_image);
                // co = StartCoroutine(MetaApiStatic.InferenceAPI(screenShot_image));
                //hungnc end
                uiMessage.text += ".";
                // var resizedImage = ImageProcessing.ResizeTexture(TakeScreenshot_demo(), 640,480);
                // co = StartCoroutine(MetaApiStatic.InferenceAPI(resizedImage.EncodeToPNG()));
                // StartCoroutine(CaptureCameraScreenshotCoroutine_fail());
                // co = StartCoroutine(MetaApiStatic.InferenceAPI(cameraImage));
            }
        }
        catch (UnityException ex)
        {
            UnityEngine.Debug.LogError("-----------------------" + ex.ToString());
        }
    }
    
    // Listen event from Inference response in Meta API, event parameter will be drawn in this function
    private void OnInferenceResponse(object sender, MetaApiStatic.OnInferenceResponseEventArgs e)
    {
        List<Vector2[]> metaRois = new List<Vector2[]>();
        UnityEngine.Debug.Log("START ARCameraScript/OnInferenceResponse ");
        if (StationStageIndex.FunctionIndex != "Detect"){
            return;
        }
        //Mock
        //Update world points
        // _mainCamera = mainCamera;
        //metaRois  = GetObjectScreenPointRoi();

        //Real meta
        metaAPIinferenceData = JsonConvert.DeserializeObject<InferenceResult>(e.inferenceResponse);
        // UnityEngine.Debug.Log("ARCameraScript/OnInferenceResponse 1 " + (float)(Time.time- StationStageIndex.startTime));
        try
        {
            if (!metaAPIinferenceData.data.rois.Equals(null))
            {
                // Debug.Log("ARCameraScript/OnInferenceResponse Success get rois");
                foreach (int[] roi in metaAPIinferenceData.data.rois)
                {
                    Vector2[] corner = new Vector2[2];

                    // corner[0] = new Vector2(roi[0]*Screen.width/640,(Screen.height-roi[1])*Screen.height/480);
                    // corner[1] = new Vector2(roi[2]*Screen.width/640,(Screen.height-roi[3])*Screen.height/480);

                    roi[0] = (int)(roi[0] * Screen.width / imageWidth);
                    roi[1] = (int)(roi[1] * Screen.height / imageHeight);
                    roi[2] = (int)(roi[2] * Screen.width / imageWidth);
                    roi[3] = (int)(roi[3] * Screen.height / imageHeight);


                    corner[0] = new Vector2(roi[0], (Screen.height - roi[1]));
                    corner[1] = new Vector2(roi[2], (Screen.height - roi[3]));
                    metaRois.Add(corner);
                }
                roisClassId = metaAPIinferenceData.data.class_ids;
                // meta rois to world
                metaRoisWolrd = Rois2Dto3D(metaRois);
                // UnityEngine.Debug.Log("ARCameraScript/OnInferenceResponse 2 " + (float)(Time.time- StationStageIndex.startTime));
            }
            else
            {
                UnityEngine.Debug.Log("ARCameraScript/OnInferenceResponse Fail get rois");
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("ERROR draw_rois occur exception during Deserialize" + ex.ToString());
        }
        inferenceResponseFlag = true;
        // CallMetaAPIinference();//hungnc
        UnityEngine.Debug.Log("END ARCameraScript/OnInferenceResponse ");
    }
    // Draw bounding box using data return from MetaAPI
    public void Draw(UnityEngine.Rect rect, GUIStyle style)
    {
        int y = Screen.height - Mathf.RoundToInt(rect.y + rect.height);
        rect.y = Screen.height - Mathf.RoundToInt(rect.y + rect.height);
        int width = Mathf.RoundToInt(rect.width);
        int height = Mathf.RoundToInt(rect.height);
        // Draw fill rectangle
        // GUI.color = outlineColor;
        // GUI.DrawTexture(new Rect(x, y, width, height), Texture2D.whiteTexture);
        // Draw outline
        // GUI.color = fillColor;
        GUI.Box(rect, "",style);
        // GUI.DrawTexture(new Rect(x, y, width, thickness), Texture2D.whiteTexture); // top
        // GUI.DrawTexture(new Rect(x, y + height - thickness, width, thickness), Texture2D.whiteTexture); // bottom
        // GUI.DrawTexture(new Rect(x, y, thickness, height), Texture2D.whiteTexture); // left
        // GUI.DrawTexture(new Rect(x + width - thickness, y, thickness, height), Texture2D.whiteTexture); // right

        
    }
    void OnGUI()
    {
        if (StationStageIndex.FunctionIndex != "Detect"){
            return;
        }
        if (metaRoisWolrd == null){
            return;
        }
        rois2D = Rois3Dto2D(metaRoisWolrd);
        rectBboxes = new List<UnityEngine.Rect>();
        for (int index = 0; index < rois2D.Count; index++){
        // foreach(var imageTargetPosition in rois2D){
            var imageTargetPosition = rois2D[index];
                // Draw a line between the two points
            UnityEngine.Rect rect = new UnityEngine.Rect((int)(imageTargetPosition[0][0]), (int)(imageTargetPosition[0][1]),(int)((imageTargetPosition[1][0]- imageTargetPosition[0][0])),(int)((imageTargetPosition[1][1]- imageTargetPosition[0][1])));
            
            var borderSize = -1; // Border size in pixels
            var style = new GUIStyle();
        //Initialize RectOffset object
            style.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);
            // if (ConfigRead.projectData.project_rule.State_1.model.class_name[roisClassId[index]].color.Contains("#0")){
            if (MetaApiStatic.metaResponseData.data[StationStageIndex.stageIndex-1].model.class_name[roisClassId[index]].color.Contains("#0")){
                style.normal.background = GreenBoxBorder;
            }
            else{
                style.normal.background = RedBoxBorder;
            }
            
            int x = Mathf.RoundToInt(rect.x);
            rectBboxes.Add(rect);
            Draw(rect, style);
        }
    }
    void OnVuforiaStarted()
    {
        mTexture = new Texture2D(0, 0, TEXTURE_FORMAT, false);
        // A format cannot be registered if Vuforia Engine is not running
        RegisterFormat();
    }
    void RegisterFormat()
    {
        // Vuforia Engine has started, now register camera image format
        var success = VuforiaBehaviour.Instance.CameraDevice.SetFrameFormat(PIXEL_FORMAT, true);
        if (success)
        {
            //Debug.Log("Successfully registered pixel format " + PIXEL_FORMAT);
            mFormatRegistered = true;
        }
        else
        {
            UnityEngine.Debug.LogError("Failed to register pixel format " + PIXEL_FORMAT +
                           "\n the format may be unsupported by your device;" +
                           "\n consider using a different pixel format.");
            mFormatRegistered = false;
        }
    }
    public Texture2D GetCameraImageLoc()
    {
        Vuforia.Image image = VuforiaBehaviour.Instance.CameraDevice.GetCameraImage(PIXEL_FORMAT);
        if (image == null)
        {
            print("nullll----------------------------------");
        }
        image.CopyToTexture(mTexture, true);
        copyTexture = new Texture2D(mTexture.width, mTexture.height);
        copyTexture.SetPixels(mTexture.GetPixels());
        copyTexture.Apply();
        ImageProcessing.SaveImageToAssets(copyTexture.EncodeToPNG());
        copyTexture = ImageProcessing.crop_image(copyTexture);
        copyTexture = ImageProcessing.AddBackgroundToImage(copyTexture);
        return copyTexture;
    }
    //Temporary function
    // private List<Vector2[]> GetObjectScreenPointRoi(){
    //     List<Vector2[]> metaRois = new List<Vector2[]>();
    //     var dim_x = StationStageIndex.stageMeshRenderBoundSize[0];
    //     var dim_y = StationStageIndex.stageMeshRenderBoundSize[2];
    //     var dim_z = StationStageIndex.stageMeshRenderBoundSize[1]/2;
    //     Vector3 newXZYVector = new Vector3(StationStageIndex.stagePosition.x, StationStageIndex.stagePosition.z, StationStageIndex.stagePosition.y);
    //     Vector3 Corner1Pt3d = new Vector3(- dim_x/ 2, dim_z, -dim_y / 2) + newXZYVector;
    //     Vector3 Corner2Pt3d = new Vector3(-dim_x / 2, dim_z, dim_y / 2)+ newXZYVector;
    //     Vector3 Corner3Pt3d = new Vector3(dim_x / 2, dim_z, dim_y / 2) + newXZYVector;
    //     Vector3 Corner4Pt3d = new Vector3(dim_x / 2, dim_z, -dim_y / 2) + newXZYVector;
    //     // Vector2 Corner1Pt2d = mainCamera.WorldToScreenPoint(Corner1Pt3d);
    //     Vector3 Corner2Pt2d = mainCamera.WorldToScreenPoint(Corner2Pt3d);
    //     Corner2Pt3d = mainCamera.ScreenToWorldPoint(new Vector3(Corner2Pt2d.x, Corner2Pt2d.y, 0.75f));
    //     Vector3 Corner4Pt2d = mainCamera.WorldToScreenPoint(Corner4Pt3d);
    //     // Debug.Log("");
    //     corner_distance = new float[] {Corner2Pt2d.z, Corner4Pt2d.z};
    //     metaRois.Add(new Vector2[] {Corner2Pt2d, Corner4Pt2d });
    //     return  metaRois;
    // }

    private List<Vector3[]> Rois2Dto3D(List<Vector2[]> rois){
        List<Vector3[]> metaRois3D = new List<Vector3[]>();
        for (int i = 0; i <  rois.Count; i++){
            Vector3 corner1World3D = _mainCamera.ScreenToWorldPoint(new Vector3(rois[i][0].x, rois[i][0].y, _mainCamera.WorldToScreenPoint(new Vector3(0f,0f,0f)).z));
            Vector3 corner2World3D = _mainCamera.ScreenToWorldPoint(new Vector3(rois[i][1].x, rois[i][1].y, _mainCamera.WorldToScreenPoint(new Vector3(0f,0f,0f)).z));
            metaRois3D.Add(new Vector3[]{corner1World3D, corner2World3D});
        }
        return  metaRois3D;
    }
    private List<Vector2[]> Rois3Dto2D(List<Vector3[]> rois3D){
        List<Vector2[]> metaRois2D = new List<Vector2[]>();
        foreach (Vector3[] roi in rois3D){
            Vector2 corner1World3D = mainCamera.WorldToScreenPoint(roi[0]);
            Vector2 corner2World3D = mainCamera.WorldToScreenPoint(roi[1]);
            metaRois2D.Add(new Vector2[]{corner1World3D, corner2World3D});
        }
        return  metaRois2D;
    }
    public Texture2D TakeScreenshot_demo()
    {
        _mainCamera.CopyFrom(mainCamera);//(Camera.main);
        // Set the camera's target texture to the render texture
        Camera.main.targetTexture = renderTexture;
        // Render the scene
        Camera.main.Render();
        // Read the pixels from the render texture
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new UnityEngine.Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // Reset the target texture and active render texture
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        // Apply the pixels to the texture
        screenShot.Apply();
        return screenShot;//ImageProcessing.ResizeTexture(screenShot, 640, 480);
    }

    // private void CallCaptureScreen(string functionName){
    //     // thread = new Thread(GetCameraDataAndControlMeta);
    //     // thread.Start();
    //     StartCoroutine(GetCameraDataAndControlMeta());
    // }

    public IEnumerator GetCameraDataAndControlMeta(){
        inferenceResponseFlag = true;
        while (StationStageIndex.FunctionIndex == "Detect"){
            if (inferenceResponseFlag) {
                inferenceResponseFlag = false;
                string ss = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                _mainCamera.CopyFrom(mainCamera);
                // _mainCamera = Instantiate(mainCamera);
                // _mainCamera = GameObject.Instantiate(mainCamera);
                // _mainCamera.transform.position = mainCamera.transform.position;
                // _mainCamera.transform.rotation = mainCamera.transform.rotation;
                // _mainCamera.rect = mainCamera.rect;
                // _mainCamera.projectionMatrix = mainCamera.projectionMatrix;

                // Copy any additional camera properties here, such as the field of view
                // _mainCamera.fieldOfView = mainCamera.fieldOfView;


                stopwatch.Stop(); ss += $"copy: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                screenShot.ReadPixels(new UnityEngine.Rect(0, 0, Screen.width, Screen.height), 0, 0);
                stopwatch.Stop(); ss += $"ReadPixels: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                screenShot.Apply();
                stopwatch.Stop(); ss += $"Apply: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                stopwatch.Stop(); ss += $"EncodeToPNG: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                stopwatch.Stop(); ss += $"Invoke: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                //MetaApiStatic.InferenceAPI(screenShot);
                stopwatch.Stop(); ss += $"InferenceAPI: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                UnityEngine.Debug.Log(ss);
                yield return null;
            }
        }
    }
    public void OnPostRenderCallback(Camera cam)
    {
        if (StationStageIndex.FunctionIndex == "Detect")
        {
            if (inferenceResponseFlag) {
                inferenceResponseFlag = false;
                string ss = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                _mainCamera.CopyFrom(mainCamera);
                // stopwatch.Stop(); ss += $"copy: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();

                // screenShot.ReadPixels(new UnityEngine.Rect(0, 0, 640, 480), 0, 0);
                // // stopwatch.Stop(); ss += $"ReadPixels: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                // screenShot.Apply();
                // // stopwatch.Stop(); ss += $"Apply: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                // // stopwatch.Stop(); ss += $"EncodeToPNG: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                // screenShot_image = screenShot.EncodeToJPG();


                renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
                _mainCamera.targetTexture = renderTexture;
                // Render the scene
                _mainCamera.Render();
                // Read the pixels from the render texture
                RenderTexture.active = renderTexture;
                screenShot.ReadPixels(new UnityEngine.Rect(0, 0, imageWidth, imageHeight), 0, 0);
                // Reset the target texture and active render texture
                // Apply the pixels to the texture
                screenShot.Apply();
                screenShot_image = screenShot.EncodeToJPG();

                // // convert the colors to bytes using Buffer.BlockCopy
                // ImageProcessing.SaveImageToAssets(screenShot_image);

                // byte[] bytes = ImageConversion.EncodeArrayToJPG(screenShot.GetRawTextureData(), screenShot.graphicsFormat, 640, 480);
                // ImageProcessing.SaveImageToAssets(bytes);
                // Mat matImage = OpenCvSharp.Unity.TextureToMat(screenShot);
                // ImageEncodingParam[] encodingParams = new ImageEncodingParam[]
                // {
                //     new ImageEncodingParam(ImwriteFlags.JpegQuality, 90)
                // };
                // byte[] imageInBytes = matImage.ToBytes(".jpg", new[] { new ImageEncodingParam(ImwriteFlags.JpegQuality, 100) });//new byte[matImage.Total() * matImage.Channels()];//ImreadModes
                stopwatch.Stop(); ss += $"EncodeToJPG: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                // UnityEngine.Debug.Log(ss);
                MetaApiStatic.InferenceAPI(screenShot_image);
                stopwatch.Stop(); ss += $"InferenceAPI: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                // stopwatch.Stop(); ss += $"null: {stopwatch.ElapsedMilliseconds}, "; stopwatch.Restart();
                UnityEngine.Debug.Log(ss);
                _mainCamera.targetTexture = null;
                RenderTexture.active = null;
                RenderTexture.Destroy(renderTexture);
            }
        }
    }

    private static byte[] Color32ArrayToByteArray(Color32[] colors)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = lengthOfColor32 * colors.Length;
        byte[] bytes = new byte[length];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }

    public bool TakeScreenshot()
    {
        // webCamTexture.Stop();
        // Draw on Image hungnc
        #if UNITY_ANDROID && !UNITY_EDITOR
        capture_image.texture = ScreenCapture.CaptureScreenshotAsTexture();
        #else
        screenShot.LoadImage(screenShot_image);
        capture_image.texture = screenShot;
        #endif
        
        if (!metaAPIinferenceData.data.rule.Equals(null)){
            return metaAPIinferenceData.data.rule;
        }
        return false;
    }

    // public void DrawRectangles(List<Rect> rectangles, Texture2D texture)
    // {
    //     using (var graphics = Graphics.FromImage(texture))
    //     {
    //         var pen = new Pen(Color.Red, 2);

    //         foreach (var rect in rectangles)
    //         {
    //             graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
    //         }
    //     }
    // }

    public void OnlineCamera(){
        gameObject.SetActive(true);
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
    }
}
