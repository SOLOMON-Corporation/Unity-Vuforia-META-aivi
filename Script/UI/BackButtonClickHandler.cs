using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackButtonClickHandler : MonoBehaviour
{
    public Button backButton;
    public ARCameraScript arCameraScript;
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    // public EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(RaiseButtonClick);
    }

    void OnDisable(){
        // Debug.Log(gameObject.name + "-----------> OnDisable");
        // EventManager.OnStageChange -= OnPlayAnimation;
        backButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        // Debug.Log(gameObject.name + "-----------> OnEnable");
        // EventManager.OnStageChange += OnPlayAnimation;
        backButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick()
    {
        // Change stage index and raise the event
        // Debug.Log("----------RaiseButtonClick!");
        // arCameraScript.TakeScreenshot();
        string jump2StageName = "";
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;

        // if (StationStageIndex.functionIndex == 1){
        //     uiMessage.text = "Animation => ";
        //     StationStageIndex.functionIndex -= 1;
        // }
        // else{
        //     uiMessage.text = "MetaAI => ";
        //     StationStageIndex.stageIndex -= 1;
        //     StationStageIndex.functionIndex = 1;
        //     arCameraScript.CallMetaAPIinference();

        // }
        StationStageIndex.stageIndex -= 1;
        StationStageIndex.FunctionIndex = "Sample";
        if (StationStageIndex.stageIndex <= 0){
            StationStageIndex.stageIndex = 0;
            StationStageIndex.FunctionIndex = "VuforiaTarget";
            backButton.gameObject.SetActive(false);
            return;
        }
        if (MetaApiStatic.triggerAPIresponseData != null){
            MetaApiStatic.triggerAPIresponseData.requestResult = false;
        }
        MetaApiStatic.ConnectMetaBasedStage();// connect meta in advance
        foreach(Datastage dataStage in dataStages){
            if (dataStage.Agrs.Order == StationStageIndex.stageIndex){
                jump2StageName = dataStage.StageName;
                StationStageIndex.UpdateMargin(dataStage);
                break;
            }
        }
        if (jump2StageName == ""){
            return;
        }
        uiMessage.text = $"{StationStageIndex.stageIndex}/{dataStages.Count -1} {jump2StageName}";
        StationStageIndex.stageName = jump2StageName;//Duplicate code
        EventManager.OnStageChange?.Invoke(this, new EventManager.OnStageIndexEventArgs{
            // stageIndex = StationStageIndex.stageIndex,
            nextButtonClick = true,
            stageName = jump2StageName
        });
        Debug.Log("back button click " + StationStageIndex.stageIndex + StationStageIndex.FunctionIndex + jump2StageName);
    }
}
