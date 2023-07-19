using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackButtonClickHandler : MonoBehaviour
{
    public Button backButton;
    [SerializeField] private TMPro.TextMeshProUGUI uiMessage;

    void OnDisable()
    {
        backButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable()
    {
        backButton.onClick.AddListener(RaiseButtonClick);
    }

    // Handle back button click event
    private void RaiseButtonClick()
    {
        string jump2StageName = "";
        List<Datastage> dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        StationStageIndex.stageIndex -= 1;

        if (StationStageIndex.stageIndex <= 0)
        {
            // If at the first stage, set necessary values and hide the back button
            StationStageIndex.stageIndex = 0;
            StationStageIndex.FunctionIndex = "VuforiaTarget";
            backButton.gameObject.SetActive(false);
            return;
        }
        else
        {
            // Set the function index and update the UI message
            StationStageIndex.FunctionIndex = "Sample";
            uiMessage.text = $"Instruction {StationStageIndex.stageIndex}/{dataStages.Count - 1}";
        }

        if (MetaService.stageData != null)
        {
            MetaService.stageData.requestResult = false;
        }

        MetaService.ConnectWithMetaStageID(); // Connect to meta in advance

        // Find the corresponding data stage based on the current stage index
        foreach (Datastage dataStage in dataStages)
        {
            if (dataStage.Agrs.Order == StationStageIndex.stageIndex)
            {
                jump2StageName = dataStage.StageName;
                StationStageIndex.UpdateMargin(dataStage);
                break;
            }
        }

        if (jump2StageName == "")
        {
            return;
        }

        StationStageIndex.stageName = jump2StageName;

        // Invoke the stage change event
        EventManager.OnStageChange?.Invoke(this, new EventManager.OnStageIndexEventArgs
        {
            nextButtonClick = true,
            stageName = jump2StageName
        });

        Debug.Log("Back button clicked. Stage Index: " + StationStageIndex.stageIndex + ", Function Index: " + StationStageIndex.FunctionIndex + ", Jump to Stage: " + jump2StageName);
    }
}
