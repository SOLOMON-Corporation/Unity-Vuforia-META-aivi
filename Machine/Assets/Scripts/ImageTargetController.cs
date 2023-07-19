using UnityEngine;

public class ImageTargetController : MonoBehaviour
{
    private DefaultObserverEventHandler observer;

    void OnDisable(){
        observer.OnTargetFound.RemoveListener(OnTargetFound);
        observer.OnTargetLost.RemoveListener(OnTargetLost);
    }
    void OnEnable(){
        observer = FindObjectOfType<DefaultObserverEventHandler>();
        observer.OnTargetFound.AddListener(OnTargetFound);
        observer.OnTargetLost.AddListener(OnTargetLost);
    }

    private void OnTargetFound()
    {
        StationStageIndex.ImageTargetFound = true;
    }
    private void OnTargetLost()
    {
        StationStageIndex.ImageTargetFound = false;
    }
}
