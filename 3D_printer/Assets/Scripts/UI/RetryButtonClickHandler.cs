using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RetryButtonClickHandler : MonoBehaviour
{
    public Button FinishButtonButton;

    void OnDisable(){
        FinishButtonButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        FinishButtonButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick(){
        StationStageIndex.FunctionIndex = "VuforiaTarget";
    }
}
