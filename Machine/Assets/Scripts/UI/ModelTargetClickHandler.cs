using UnityEngine;
using UnityEngine.UI;

public class ModelTargetClickHandler : MonoBehaviour
{
    public Button modelTarget;

    void OnDisable()
    {
        modelTarget.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable()
    {
        modelTarget.onClick.AddListener(RaiseButtonClick);
    }

    // This method is called when the button is clicked
    private void RaiseButtonClick()
    {
        // Set the StationStageIndex.FunctionIndex to "ScanBarcode"
        StationStageIndex.FunctionIndex = "ScanBarcode";
    }
}
