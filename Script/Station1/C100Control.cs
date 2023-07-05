using UnityEngine;
using System;
public class C100Control : MonoBehaviour//, ITrackableEventHandler // OnTargetFound
{
    private DefaultObserverEventHandler observer;
    public static event EventHandler OnFinishAnimation;
    private Animator animator;
    void Start()
    {
        // Debug.Log("================> C100Control");
        // Listen 140 raise event
        animator = GetComponent<Animator>();
        // c140Control.OnFinish140Animation.AddListener(PlayAnimation);
        // observer = FindObjectOfType<DefaultObserverEventHandler>();
        // observer.OnTargetFound.AddListener(PlayAnimation);
        // C140Control.OnFinish140Animation += PlayAnimation;
        EventManager.OnStageChange += OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChange;
    }

    void OnDisable(){
        Debug.Log(gameObject.name + "-----------> OnDisable");
        EventManager.OnStageChange -= OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange -= OnFunctionIndexChange;
    }

    void OnEnable(){
        Debug.Log(gameObject.name + "-----------> OnEnable");
        EventManager.OnStageChange += OnPlayAnimation;
        StationStageIndex.OnFunctionIndexChange += OnFunctionIndexChange;
    }

    // private void AlertObservers(string message)
    // {
    //     if (message.Equals("AttackAnimationEnded"))
    //     {
    //         Debug.Log("Stop-----------");
    //         OnFinishAnimation?.Invoke(this, EventArgs.Empty);
    //         // animator.Play("100C_Animation", -1, 0f);
    //         // animator.Play("100-C_main");
    //         // Do other things based on an attack ending.
    //     }
    // }

    private void OnPlayAnimation(object sender, EventManager.OnStageIndexEventArgs e){
        Debug.Log("Stage Name = " + e.stageName);
        if (StationStageIndex.FunctionIndex == "Sample" && (gameObject.name == e.stageName || gameObject.name.Contains(e.stageName)))// && StationStageIndex.FunctionIndex == "Stage3D"){
        {
            Debug.Log("OnPlayAnimation");
            gameObject.SetActive(true);
            animator.enabled = true;
            // animator.Play("100C_Animation", -1, 0f);
            // gameObject.GetComponent<Animation>().Play();
            // Object size for crop part
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            StationStageIndex.stageMeshRenderBoundSize = meshRenderer.bounds.size;
            StationStageIndex.stagePosition = new Vector3(gameObject.transform.position[0], gameObject.transform.position[2], 0) ;
            // Debug.Log("0329 Debug " + gameObject.name + StationStageIndex.stageMeshRenderBoundSize + StationStageIndex.stagePosition);
        }
        else{
            //gameObject.GetComponent<Animation>().Stop();
            animator.enabled = false;
            gameObject.SetActive(false);
        }
    }
    private void OnFunctionIndexChange(string functionIndex){
        Debug.Log(functionIndex);
        if (functionIndex == "VuforiaTarget")
        {
            animator.enabled = false;
            gameObject.SetActive(true);
        }
        else{
            animator.enabled = false;
            gameObject.SetActive(false);
        }
        if (functionIndex == "Sample" && StationStageIndex.ImageTargetFound && gameObject.name == StationStageIndex.stageName)
        {
            Debug.Log("OnFunctionIndexChange");
            gameObject.SetActive(true);
            animator.enabled = true;
            // gameObject.GetComponent<Animation>().Play();
        }
    }
}
