using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class NextButtonClickHandler : MonoBehaviour
{
    public Button nextButton;
    // public ARCameraScript arCameraScript;
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    // public EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        nextButton.onClick.AddListener(RaiseButtonClick);
    }

    void OnDisable(){
        // Debug.Log(gameObject.name + "-----------> OnDisable");
        // EventManager.OnStageChange -= OnPlayAnimation;
        nextButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        // Debug.Log(gameObject.name + "-----------> OnEnable");
        // EventManager.OnStageChange += OnPlayAnimation;
        nextButton.onClick.AddListener(RaiseButtonClick);
    }

    public void Invoke()
    {
        if (nextButton != null && nextButton.onClick != null)
            nextButton.onClick.Invoke();
    }
    private void RaiseButtonClick()
    {
        Debug.Log("----------RaiseNextButtonClick: station stationIndex" + StationStageIndex.stationIndex);
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        string jump2StageName = "";
        StationStageIndex.stageIndex += 1;
        StationStageIndex.FunctionIndex = "Sample";
        if (StationStageIndex.stageIndex > dataStages.Count -1 ){
            StationStageIndex.stageIndex = dataStages.Count -1;
            return;
        }
        if (MetaApiStatic.triggerAPIresponseData != null){
            MetaApiStatic.triggerAPIresponseData.requestResult = false;
        }
        MetaApiStatic.ConnectMetaBasedStage();// connect meta in advance
        foreach(Datastage dataStage in dataStages){
            if (dataStage.Agrs.Order == StationStageIndex.stageIndex){
                jump2StageName = dataStage.StageName;
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
        Debug.Log("next button click" + StationStageIndex.stageIndex + StationStageIndex.FunctionIndex + jump2StageName);
    }
}
