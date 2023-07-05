using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotReport : MonoBehaviour
{
    public Button captureButton;
    public ARCameraScript arCameraScript;
    // public RawImage resultStatusImage;
    public Texture2D OKimage;
    public Texture2D NGimage;
    public Image backgroundResult;
    public GameObject titleBar;
    [SerializeField]  private TMPro.TextMeshProUGUI ResultStatus;
    // public Button redoButton;
    [SerializeField]  private TMPro.TextMeshProUGUI ResultStateTxt;
    // Start is called before the first frame update
    void Start()
    {
        captureButton.onClick.AddListener(RaiseButtonClick);
    }
    void OnDisable(){
        captureButton.onClick.RemoveListener(RaiseButtonClick);
    }
    void OnEnable(){
        captureButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick()
    {
        ResultStateTxt.text = "Result " +  StationStageIndex.stageIndex.ToString();
        //Hide all UI
        titleBar.SetActive(false);
        captureButton.gameObject.SetActive(false);
        StationStageIndex.metaInferenceRule = arCameraScript.TakeScreenshot();
        if (StationStageIndex.metaInferenceRule){
            // resultStatusImage.texture = OKimage;
            backgroundResult.color = Color.green;
            ResultStatus.text = "OK";
        }
        else{
            // resultStatusImage.texture = NGimage;
            backgroundResult.color = Color.red;
            ResultStatus.text = "NG";
        }
        //Show recently hided UI
        titleBar.SetActive(true);
        captureButton.gameObject.SetActive(true);
        //Raise Result page event
        StationStageIndex.FunctionIndex = "Result";
    }

}
