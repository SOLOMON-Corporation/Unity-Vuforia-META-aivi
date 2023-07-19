using UnityEngine;

public class CheckListController : MonoBehaviour
{
    private GameObject label;
    public GameObject highlightBackground;

    void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        EventManager.OnStageChange -= OnPlayAnimation;
    }

    void OnEnable()
    {
        // Subscribe to the event when the script is enabled
        EventManager.OnStageChange += OnPlayAnimation;
    }

    // This method is called when the stage changes
    private void OnPlayAnimation(object sender, EventManager.OnStageIndexEventArgs e)
    {
        // Find the label game object based on the current stage index
        label = GameObject.Find("CP" + StationStageIndex.stageIndex.ToString());

        // If the label is not found, return and do nothing
        if (label == null)
        {
            return;
        }

        // Adjust the position of the highlight background to match the label's position
        highlightBackground.transform.position = new Vector3(
            highlightBackground.transform.position.x,
            label.transform.position.y,
            highlightBackground.transform.position.z);
    }
}
