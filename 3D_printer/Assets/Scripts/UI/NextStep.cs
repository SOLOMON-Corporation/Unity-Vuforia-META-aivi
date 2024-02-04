using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStep : MonoBehaviour
{
    public Button nextStep;
    [SerializeField] private TMPro.TextMeshProUGUI uiMessage;

    private void OnDisable()
    {
        nextStep.onClick.RemoveListener(RaiseButtonClick);
    }

    private void OnEnable()
    {
        nextStep.onClick.AddListener(RaiseButtonClick);
    }

    // Handle button click event
    private void RaiseButtonClick()
    {
        switch (StationStageIndex.FunctionIndex)
        {
            case "3dModel":
                // Show single 3D model
                EventManager.OnModelChangeButtonClick?.Invoke(this, new EventManager.OnModelChangeButtonClickEventArgs
                {
                    buttonType = true
                });
                break;
            case "VuforiaTarget":
                GotoNextState();
                break;
            case "Sample":
                StationStageIndex.FunctionIndex = "Detect";
                break;
            case "Detect":
                GotoNextState();
                break;
            case "Result":
                // Save to Fiix
                // Go to next state
                GotoNextState();
                break;
            case "ScanBarcode":
                StationStageIndex.FunctionIndex = "VuforiaTarget";
                break;
            default:
                break;
        }
    }

    // Go to the next state
    private void GotoNextState()
    {
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        string jump2StageName = "";

        StationStageIndex.stageIndex += 1;
        if (StationStageIndex.stageIndex > dataStages.Count - 1)
        {
            StationStageIndex.stageIndex = dataStages.Count - 1;
            return;
        }
        else
        {
            StationStageIndex.FunctionIndex = "Sample";
        }

        if (MetaService.stageData != null)
        {
            MetaService.stageData.requestResult = false;
        }

        MetaService.ConnectWithMetaStageID(); // Connect meta in advance

        foreach (Datastage dataStage in dataStages)
        {
            if (dataStage.Agrs.Order == StationStageIndex.stageIndex)
            {
                jump2StageName = dataStage.StageName;
                break;
            }
        }

        if (jump2StageName == "")
        {
            return;
        }

        StationStageIndex.stageName = jump2StageName; // Duplicate code

        EventManager.OnStageChange?.Invoke(this, new EventManager.OnStageIndexEventArgs
        {
            nextButtonClick = true,
            stageName = jump2StageName
        });
    }
}
