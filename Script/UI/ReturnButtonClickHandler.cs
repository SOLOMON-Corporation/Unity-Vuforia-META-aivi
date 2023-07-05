using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ReturnButtonClickHandler : MonoBehaviour
{
    public Button returnButton;
    // Start is called before the first frame update
    void Start()
    {
        returnButton.onClick.AddListener(RaiseButtonClick);
    }
    // void OnDisable(){
    //     returnButton.onClick.RemoveListener(RaiseButtonClick);
    // }

    // void OnEnable(){
    //     returnButton.onClick.AddListener(RaiseButtonClick);
    // }
    private void RaiseButtonClick(){
        int index = Array.IndexOf(StationStageIndex.functionList, StationStageIndex.FunctionIndex);
        index = index - 1;
        if (index <= 0){
            return;
        }
        StationStageIndex.FunctionIndex = StationStageIndex.functionList[index];
    }
}
