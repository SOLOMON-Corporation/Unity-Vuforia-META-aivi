using UnityEngine;
using UnityEngine.UI;

public class FinishButtonClickHandler : MonoBehaviour
{
    public Button FinishButtonButton;

    void OnDisable()
    {
        // Remove the event listener when the object is disabled
        FinishButtonButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable()
    {
        // Add the event listener when the object is enabled
        FinishButtonButton.onClick.AddListener(RaiseButtonClick);
    }

    // This method is called when the button is clicked
    private void RaiseButtonClick()
    {
        // Set the function index to "ScanBarcode"
        StationStageIndex.FunctionIndex = "ScanBarcode";
    }
}
