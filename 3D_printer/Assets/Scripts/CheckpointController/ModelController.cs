using UnityEngine;
using System.Collections.Generic;

public class ModelController : MonoBehaviour
{
    private DefaultObserverEventHandler observer;
    private MeshRenderer meshRenderer;
    private List<Datastage> dataStages;

    void Start()
    {
        // Subscribe to events
        EventManager.OnStageChange += On3DModelTrigger;
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChange;

        // Get MeshRenderer component
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    // Event handler for the OnStageChange event
    private void On3DModelTrigger(object sender, EventManager.OnStageIndexEventArgs e)
    {
        Debug.Log("Stage Name = " + e.stageName);

        // Check if the stage name matches or is contained in the game object's name
        if (gameObject.name == e.stageName || gameObject.name.Contains(e.stageName))
        {
            Debug.Log("OnPlayAnimation");
            gameObject.SetActive(true);

            // Store the position and bounds size of the game object
            StationStageIndex.stagePosition = gameObject.transform.position;
            StationStageIndex.stageMeshRenderBoundSize = meshRenderer.bounds.size;
        }
        else
        {
            // Hide the game object if the stage name doesn't match
            gameObject.SetActive(false);
            return;
        }

        // Store the game object's position and scale in corners array
        Vector3[] corners = new Vector3[2];
        corners[0] = gameObject.transform.position;
        corners[1] = gameObject.transform.localScale;

        // Check if dataStages is null and initialize it
        if (dataStages == null)
        {
            dataStages = ConfigRead.configData.DataStation[StationStageIndex.stationIndex].Datastage;
        }

        // Update or add the game object's corners based on its stage name
        foreach (Datastage dataStage in dataStages)
        {
            if (dataStage.StageName == gameObject.name)
            {
                if (StationStageIndex.gameObjectPoints.ContainsKey($"{dataStage.Agrs.Order}"))
                {
                    // Key exists, update the value
                    StationStageIndex.gameObjectPoints[$"{dataStage.Agrs.Order}"] = corners;
                    break;
                }
                else
                {
                    // Key does not exist, add a new key-value pair
                    StationStageIndex.gameObjectPoints.Add($"{dataStage.Agrs.Order}", corners);
                }
                break;
            }
        }
    }

    // Event handler for the OnFunctionIndexChange event
    private void OnFunctionIndexChange(string functionIndex)
    {
        Debug.Log(functionIndex);

        // Check if functionIndex is "VuforiaTarget" to activate the game object
        if (functionIndex == "VuforiaTarget")
        {
            gameObject.SetActive(true);
        }

        // Activate the game object if ImageTargetFound is true and its name matches
        if (StationStageIndex.ImageTargetFound && gameObject.name == StationStageIndex.stageName)
        {
            Debug.Log("OnFunctionIndexChange");
            gameObject.SetActive(true);
        }
    }
}
