using UnityEngine;

public class LabelControl : MonoBehaviour
{
    public Camera arCamera;

    void Start()
    {
        // Subscribe to events
        EventManager.OnStageChange += OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChange;
        StationStageIndex.OnImageTargetFoundChange += OnImageTargetFoundActionHandler;
    }

    private void OnPlayAnimation(object sender, EventManager.OnStageIndexEventArgs e)
    {
        // Check conditions to show or hide the game object
        if (StationStageIndex.FunctionIndex == "Sample" && StationStageIndex.ImageTargetFound && gameObject.name.Contains(StationStageIndex.stageIndex.ToString()))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnFunctionIndexChange(string functionIndex)
    {
        // Check conditions to show or hide the game object
        if (functionIndex == "Sample" && StationStageIndex.ImageTargetFound && gameObject.name.Contains(StationStageIndex.stageIndex.ToString()))
        {
            Debug.Log("OnFunctionIndexChange");
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnImageTargetFoundActionHandler(bool imageTargetFound)
    {
        // Check conditions to show or hide the game object
        if (imageTargetFound && StationStageIndex.FunctionIndex == "Sample" && StationStageIndex.ImageTargetFound && gameObject.name.Contains(StationStageIndex.stageIndex.ToString()))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetGameObjectHeadToSpecificPosition(GameObject sourceGameObject, GameObject destinationGameObject)
    {
        // Calculate the direction from the source object to the destination object
        Vector3 cameraDirection = sourceGameObject.transform.position - destinationGameObject.transform.position;

        // Rotate the source object to face the camera direction
        sourceGameObject.transform.rotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
    }
}
