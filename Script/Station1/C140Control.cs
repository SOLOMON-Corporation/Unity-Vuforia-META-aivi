// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using UnityEngine.Events;
// public class C140Control : MonoBehaviour
// {
//     // Start is called before the first frame update
//     private Animator animator;
//     public static event EventHandler OnFinish140Animation;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         // C100Control.OnFinishAnimation += OnPlayAnimation;
//         EventManager.OnStageChange += OnPlayAnimation;
//     }

//     private void OnPlayAnimation(object sender, EventManager.OnStageIndexEventArgs e){
//         Debug.Log("Stage Name = " + e.stageName);
//         if (gameObject.name == e.stageName && StationStageIndex.FunctionIndex == 0){
//             gameObject.SetActive(true);
//             animator.enabled = true;
//             // Debug.Log("play 100 ");
//             animator.Play("140M_Animation", -1, 0f);
//         }
//         else{
//             gameObject.SetActive(false);
//         }
//     }

//     private void AlertObservers(string message)
//     {
//         if (message.Equals("AttackAnimationEnded"))
//         {
//             Debug.Log("Stop-----------");
//             OnFinish140Animation?.Invoke(this, EventArgs.Empty);
//             // animator.Play("100C_Animation", -1, 0f);
//             // animator.Play("100-C_main");
//             // Do other things based on an attack ending.
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
