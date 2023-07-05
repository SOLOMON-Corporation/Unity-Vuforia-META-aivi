using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinishButtonClickHandler : MonoBehaviour
{
    public Button FinishButtonButton;
    void Start()
    {
        FinishButtonButton.onClick.AddListener(RaiseButtonClick);
    }

    void OnDisable(){
        FinishButtonButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        FinishButtonButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick(){
        StationStageIndex.FunctionIndex = "ScanBarcode";
    }
}
