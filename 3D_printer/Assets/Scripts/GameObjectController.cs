using UnityEngine;
using System.Diagnostics;

public class GameObjectController : MonoBehaviour
{
    public GameObject barcode;
    public ARCameraScript arCameraScript;
    void Start()
    {
        StationStageIndex.OnFunctionIndexChange += OnGameObjectControllerFunctionChangeHandler;
    }
    void OnDisable(){
        StationStageIndex.OnFunctionIndexChange -= OnGameObjectControllerFunctionChangeHandler;
    }
    void OnEnable(){
        StationStageIndex.OnFunctionIndexChange += OnGameObjectControllerFunctionChangeHandler;
    }
    private void OnGameObjectControllerFunctionChangeHandler(string functionName){
        if (StationStageIndex.metaTimeCount != null){
            StationStageIndex.metaTimeCount.Stop();
            //Add to total time
            StationStageIndex.metaTotalMinute += StationStageIndex.metaTimeCount.Elapsed.Minutes;
            StationStageIndex.metaTotalSecond += StationStageIndex.metaTimeCount.Elapsed.Seconds;
            StationStageIndex.metaTempMinute = StationStageIndex.metaTimeCount.Elapsed.Minutes;
            StationStageIndex.metaTempSecond = StationStageIndex.metaTimeCount.Elapsed.Seconds;
            StationStageIndex.metaTimeCount = null;
            if (StationStageIndex.metaTotalSecond > 60){
                StationStageIndex.metaTotalSecond -= 60;
                StationStageIndex.metaTotalMinute += 1;
            }
        }
        switch (functionName){
            case "Home":// 2 button: Main demo or show 3d model. future approach: using vuforia area target in background
                barcode.SetActive(false);
                break;
            case "ScanBarcode":// Show square bounding box
                StationStageIndex.barcodeFiixOn = false;
                StationStageIndex.barcodeMetaOn = false;
                StationStageIndex.stageIndex = 0;
                barcode.SetActive(true);
                break;
            case "VuforiaTarget":// Image target: all 3D model show up
                barcode.SetActive(false);
                StationStageIndex.stageIndex = 0;
                break;
            case "Sample":// show single 3D model
                break;
            case "Detect":
                StationStageIndex.metaInferenceRule = false;
                ARCameraScript.inferenceResponseFlag = true;
                StationStageIndex.FinalUI = false;
                StationStageIndex.metaTimeCount = new Stopwatch();
                StationStageIndex.metaTimeCount.Start();
                break;
            case "Result":
                break;
            default:
                break;
        }
    }
}
