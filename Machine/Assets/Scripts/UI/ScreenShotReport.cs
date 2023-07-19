using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotReport : MonoBehaviour
{
    public Button captureButton;
    public ARCameraScript arCameraScript;
    // public Button redoButton;
    [SerializeField]  private TMPro.TextMeshProUGUI ResultStateTxt;
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    // Start is called before the first frame update
    void OnDisable(){
        captureButton.onClick.RemoveListener(RaiseButtonClick);
    }
    void OnEnable(){
        captureButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick()
    {
        uiMessage.text = "";
        ResultStateTxt.text = "Result " +  StationStageIndex.stageIndex.ToString();
        arCameraScript.TakeScreenshot();
        //Raise Result page event
        StationStageIndex.FunctionIndex = "Result";
    }

}
