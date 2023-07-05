using UnityEngine;
using System;
public class LabelControl : MonoBehaviour//, ITrackableEventHandler // OnTargetFound
{
    void Start()
    {
        EventManager.OnStageChange += OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChange;
    }

    void OnDisable(){
        EventManager.OnStageChange -= OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange -= OnFunctionIndexChange;
    }

    void OnEnable(){
        EventManager.OnStageChange += OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChange;
    }

    private void OnPlayAnimation(object sender, EventManager.OnStageIndexEventArgs e){
        if (StationStageIndex.FunctionIndex == "Sample" && StationStageIndex.ImageTargetFound && gameObject.name.Contains(StationStageIndex.stageIndex.ToString()))// && StationStageIndex.FunctionIndex == "Stage3D"){
        {
            gameObject.SetActive(true);
        }
        else{
            gameObject.SetActive(false);
        }
    }
    private void OnFunctionIndexChange(string functionIndex){
        if (functionIndex == "Sample" && StationStageIndex.ImageTargetFound && gameObject.name.Contains(StationStageIndex.stageIndex.ToString()))
        {
            Debug.Log("OnFunctionIndexChange");
            gameObject.SetActive(true);
        }
        else{
            gameObject.SetActive(false);
        }
    }
}
