using UnityEngine;
using UnityEngine.UI;
using System;
public class ReturnButtonClickHandler : MonoBehaviour
{
    public Button returnButton;
    void OnDisable(){
        returnButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        returnButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick(){
        int index = Array.IndexOf(StationStageIndex.functionList, StationStageIndex.FunctionIndex);
        index = index - 1;
        if (index <= 0){
            StationStageIndex.FunctionIndex = "Home";
        }
        else
        {
            StationStageIndex.FunctionIndex = StationStageIndex.functionList[index];
        }
    }
}
