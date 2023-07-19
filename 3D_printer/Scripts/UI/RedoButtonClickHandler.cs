using UnityEngine;
using UnityEngine.UI;
public class RedoButtonClickHandler : MonoBehaviour
{
    public Button redoButton;

    void OnDisable(){
        redoButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        redoButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick(){
        StationStageIndex.FunctionIndex = "Detect";
    }
}
