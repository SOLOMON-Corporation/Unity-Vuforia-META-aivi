using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NextToMetaFiix : MonoBehaviour
{
    public Button nextMetaFiixButton;
    public Button nextButton;
    // public NextButtonClickHandler nextButtonClickHandler;
    void Start()
    {
        nextMetaFiixButton.onClick.AddListener(RaiseButtonClick);
    }

    void OnDisable(){
        // Debug.Log(gameObject.name + "-----------> OnDisable");
        // EventManager.OnStageChange -= OnPlayAnimation;
        nextMetaFiixButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        // Debug.Log(gameObject.name + "-----------> OnEnable");
        // EventManager.OnStageChange += OnPlayAnimation;
        nextMetaFiixButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick(){
        switch (StationStageIndex.FunctionIndex){
            case "Sample":// show single 3D model
                StationStageIndex.FunctionIndex = "Detect";
                break;
            case "Result":
                //Save to Fiix
                // go to next state
                GotoNextState();
                break;
            case "ScanBarcode":
                StationStageIndex.FunctionIndex = "VuforiaTarget";
                break;
            default:
                break;
        }
    }
    //Duplicate code because nextButton.onClick.Invoke(); seem not work
    private void GotoNextState()
    {
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        string jump2StageName = "";

        StationStageIndex.stageIndex += 1;
        StationStageIndex.FunctionIndex = "Sample";
        if (StationStageIndex.stageIndex > dataStages.Count -1 ){
            StationStageIndex.stageIndex = dataStages.Count -1;
            return;
        }
        if (StationStageIndex.stageIndex == dataStages.Count -1 ){
            nextButton.gameObject.SetActive(false);
        } 
        else{
            nextButton.gameObject.SetActive(true);
        }
        foreach(Datastage dataStage in dataStages){
            if (dataStage.Agrs.Order == StationStageIndex.stageIndex){
                jump2StageName = dataStage.StageName;
                break;
            }
        }
        if (jump2StageName == ""){
            return;
        }
        StationStageIndex.stageName = jump2StageName;//Duplicate code
        EventManager.OnStageChange?.Invoke(this, new EventManager.OnStageIndexEventArgs{
            nextButtonClick = true,
            stageName = jump2StageName
        });
    }


}
