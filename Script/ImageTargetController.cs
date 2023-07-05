using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetController : MonoBehaviour
{
    private DefaultObserverEventHandler observer;
    void Start()
    {
        observer = FindObjectOfType<DefaultObserverEventHandler>();
        observer.OnTargetFound.AddListener(OnTargetFound);
        observer.OnTargetLost.AddListener(OnTargetLost);
    }

    void OnDisable(){
        observer.OnTargetFound.RemoveListener(OnTargetFound);
        observer.OnTargetLost.RemoveListener(OnTargetLost);
    }
    void OnEnable(){
        observer.OnTargetFound.AddListener(OnTargetFound);
        observer.OnTargetLost.AddListener(OnTargetLost);
    }

    private void OnTargetFound()
    {
        // Debug.Log("START OnTargetFound");
        // if (metaAPI.triggerAPIresponseData.requestResult && StationStageIndex.barcodeMetaOn)
        // {
        //     // Debug.Log("StartCoroutine");
        //     // CallMetaAPIinference();
        //     uiMessage.text = "";
        // }
        StationStageIndex.ImageTargetFound = true;
    }
    private void OnTargetLost()
    {
        Debug.Log("Target Lost!");
        StationStageIndex.ImageTargetFound = false;
    }
}
