using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class UIController : MonoBehaviour
{
    public Button nextButton;
    public Button backButton;
    public Button screenShotButton;
    public Button redoButton;
    public Button sendToFiixButton;
    public Button fiixOn;
    public Button MetaOn;
    public GameObject qrCodeFrame;
    public Button finishButton;
    // public RawImage capture_image;
    public GameObject BottomBackground;
    public GameObject ResultCanvas;
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;

    // Start is called before the first frame update
    void Start()
    {
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChangeActionHandler;
        StationStageIndex.OnImageTargetFoundChange += OnImageTargetFoundActionHandler;
    }

    // Update is called once per frame
    private void OnFunctionIndexChangeActionHandler(string functionName){
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        switch (functionName){
            case "Home":// 2 button: Main demo or show 3d model. future approach: using vuforia area target in background
                backButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                screenShotButton.gameObject.SetActive(false);
                redoButton.gameObject.SetActive(false);
                sendToFiixButton.gameObject.SetActive(false);
                fiixOn.gameObject.SetActive(false);
                MetaOn.gameObject.SetActive(false);
                break;
            case "ScanBarcode":// Show square bounding box
                uiMessage.text = "Scan META";
                // uiMessage.text = Path.Combine(Application.streamingAssetsPath, "Config/InputParameter.json");
                // capture_image.gameObject.SetActive(false);
                qrCodeFrame.SetActive(true);
                backButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                screenShotButton.gameObject.SetActive(false);
                redoButton.gameObject.SetActive(false);
                sendToFiixButton.gameObject.SetActive(false);
                fiixOn.gameObject.SetActive(false);
                MetaOn.gameObject.SetActive(true);
                finishButton.gameObject.SetActive(false);
                BottomBackground.SetActive(false);
                ResultCanvas.SetActive(false);
                break;
            case "VuforiaTarget":// Image target: all 3D model show up
                uiMessage.text = $"{MetaApiStatic.qrMetaData[2]} \n Vuforia target detecting...";
                // uiMessage.text = Path.Combine(Application.streamingAssetsPath,$"Project/{MetaApiStatic.qrMetaData[2].Replace(" ","")}.json");
                qrCodeFrame.SetActive(false);
                fiixOn.gameObject.SetActive(false);
                MetaOn.gameObject.SetActive(false);
                sendToFiixButton.gameObject.SetActive(false);
                finishButton.gameObject.SetActive(false);
                if (StationStageIndex.ImageTargetFound){
                    // backButton.gameObject.SetActive(true);
                    nextButton.gameObject.SetActive(true);
                    uiMessage.text = $"{MetaApiStatic.qrMetaData[2]} \n Vuforia target detected!";
                }
                break;
            case "Sample":// show single 3D model
                ResultCanvas.SetActive(false);
                BottomBackground.SetActive(false);
                // capture_image.gameObject.SetActive(false);
                // uiMessage.text = ConfigRead.logDisplay.Substring(0, 5);;
                if (StationStageIndex.stageIndex >= dataStages.Count -2 ){
                    nextButton.gameObject.SetActive(false);
                }
                else{
                    nextButton.gameObject.SetActive(true);
                }
                if (StationStageIndex.stageIndex <= 0){
                    backButton.gameObject.SetActive(false);
                }
                else{
                    backButton.gameObject.SetActive(true);
                }
                sendToFiixButton.gameObject.SetActive(true);
                screenShotButton.gameObject.SetActive(false);
                uiMessage.text = $"{StationStageIndex.stageIndex}/{dataStages.Count -1}";
                break;
            case "Detect":
                uiMessage.text = $"{StationStageIndex.stageIndex}/{dataStages.Count -1} META AIVI Detecting...";
                // capture_image.gameObject.SetActive(false);
                redoButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                sendToFiixButton.gameObject.SetActive(false);
                finishButton.gameObject.SetActive(false);
                ResultCanvas.SetActive(false);
                BottomBackground.SetActive(false);
                if (MetaApiStatic.triggerAPIresponseData.requestResult){
                    screenShotButton.gameObject.SetActive(true);
                }
                else{
                    screenShotButton.gameObject.SetActive(false);
                }
                break;
            case "Result":
                uiMessage.text = $"{StationStageIndex.stageIndex}/{dataStages.Count -1} META AIVI Result";
                // capture_image.gameObject.SetActive(true);
                ResultCanvas.SetActive(true);
                BottomBackground.SetActive(true);
                screenShotButton.gameObject.SetActive(false);
                if (StationStageIndex.metaInferenceRule){
                    if (StationStageIndex.stageIndex >= dataStages.Count -1 ){
                        finishButton.gameObject.SetActive(true);
                    }
                    else{
                        sendToFiixButton.gameObject.SetActive(true);
                    }
                }
                else{
                    redoButton.gameObject.SetActive(true);
                }
                // finishButton.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            default:
                break;
        }
    }
    private void OnImageTargetFoundActionHandler(bool imageTargetFound){
        // backButton.gameObject.SetActive(true);
        if (imageTargetFound){
            nextButton.gameObject.SetActive(true);
        }
    }
}
