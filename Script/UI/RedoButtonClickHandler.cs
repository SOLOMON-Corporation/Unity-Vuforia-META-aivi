using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RedoButtonClickHandler : MonoBehaviour
{
    public Button redoButton;
    // public NextButtonClickHandler nextButtonClickHandler;
    void Start()
    {
        redoButton.onClick.AddListener(RaiseButtonClick);
    }

    void OnDisable(){
        // Debug.Log(gameObject.name + "-----------> OnDisable");
        // EventManager.OnStageChange -= OnPlayAnimation;
        redoButton.onClick.RemoveListener(RaiseButtonClick);
    }

    void OnEnable(){
        // Debug.Log(gameObject.name + "-----------> OnEnable");
        // EventManager.OnStageChange += OnPlayAnimation;
        redoButton.onClick.AddListener(RaiseButtonClick);
    }
    private void RaiseButtonClick(){
        StationStageIndex.FunctionIndex = "Detect";
    }
}
