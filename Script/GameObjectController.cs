using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;

public class GameObjectController : MonoBehaviour
{
    // public GameObject ImageTarget;
    public GameObject barcode;
    public GameObject ImageTarget;
    private DefaultObserverEventHandler observer;
    public static event EventHandler OnStartDetection;
    public ARCameraScript arCameraScript;
    // [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    // public GameObject
    // Start is called before the first frame update
    void Start()
    {
        StationStageIndex.OnFunctionIndexChange += OnGameObjectControllerFunctionChangeHandler;
        StationStageIndex.OnImageTargetFoundChange += OnGameObjectControllerImageTargetFoundHandler;
    }

    private void OnGameObjectControllerFunctionChangeHandler(string functionName){
        switch (functionName){
            case "Home":// 2 button: Main demo or show 3d model. future approach: using vuforia area target in background
                barcode.SetActive(false);
                break;
            case "ScanBarcode":// Show square bounding box
                StationStageIndex.barcodeFiixOn = false;
                StationStageIndex.barcodeMetaOn = false;
                // arCameraScript.OnlineCamera();
                barcode.SetActive(true);
                ImageTarget.SetActive(false);
                // SetActiveAllImageTargets(false);
                break;
            case "VuforiaTarget":// Image target: all 3D model show up
                barcode.SetActive(false);
                ImageTarget.SetActive(true);
                StationStageIndex.stageIndex = 0;
                // SetActiveAllImageTargets(true);
                break;
            case "Sample":// show single 3D model
                break;
            case "Detect":
                // arCameraScript.OnlineCamera();
                // MetaApiStatic.isNeedToUseInference = true;
                // OnStartDetection?.Invoke(this, EventArgs.Empty);
                ARCameraScript.inferenceResponseFlag = true;
                // StartCoroutine(arCameraScript.GetCameraDataAndControlMeta());
                // comment because this CallMetaAPIinference no longer need 
                // if (true){//(MetaApiStatic.triggerAPIresponseData.requestResult){
                //     arCameraScript.CallMetaAPIinference();
                // }
                break;
            case "Result":
                break;
            default:
                break;
        }
    }
    private void OnGameObjectControllerImageTargetFoundHandler(bool imageTargetFound){

    }
    // private void SetActiveAllImageTargets(bool imageTargetEnable){
    //     observer = FindObjectOfType<DefaultObserverEventHandler>();
    //     observer.gameObject.SetActive(imageTargetEnable);
    // }
}
