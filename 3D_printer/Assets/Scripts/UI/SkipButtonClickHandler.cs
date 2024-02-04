using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class SkipButtonClickHandler : MonoBehaviour
{
    public Button skipButton;
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;

    void OnDisable(){
        skipButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        skipButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick()
    {
        Debug.Log("----------RaiseNextButtonClick: station stationIndex" + StationStageIndex.stationIndex);
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        string jump2StageName = "";
        StationStageIndex.stageIndex += 1;
        if (StationStageIndex.stageIndex > dataStages.Count -1 ){
            StationStageIndex.stageIndex = dataStages.Count -1;
            StationStageIndex.FinalUI = true;
            return;
        }
        StationStageIndex.FunctionIndex = "Sample";
        if (MetaService.stageData != null){
            MetaService.stageData.requestResult = false;
        }
        MetaService.ConnectWithMetaStageID();// connect meta in advance
        foreach(Datastage dataStage in dataStages){
            if (dataStage.Agrs.Order == StationStageIndex.stageIndex){
                jump2StageName = dataStage.StageName;
                break;
            }
        }
        if (jump2StageName == ""){
            return;
        }
        // uiMessage.text = $"{StationStageIndex.stageIndex}/{dataStages.Count -1} {jump2StageName}";
        StationStageIndex.stageName = jump2StageName;//Duplicate code
        EventManager.OnStageChange?.Invoke(this, new EventManager.OnStageIndexEventArgs{
            // stageIndex = StationStageIndex.stageIndex,
            nextButtonClick = true,
            stageName = jump2StageName
        });
        Debug.Log("next button click" + StationStageIndex.stageIndex + StationStageIndex.FunctionIndex + jump2StageName);
    }
}
