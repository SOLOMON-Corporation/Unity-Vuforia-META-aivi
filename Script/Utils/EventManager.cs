using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventManager
{
    // Start is called before the first frame update
    public static EventHandler<OnStageIndexEventArgs> OnStageChange;
    public class OnStageIndexEventArgs: EventArgs{
        public int stageIndex { get; set; }
        public bool nextButtonClick { get; set; }
        public string stageName { get; set; }
        public int functionIndex { get; set; }
    }

    public static EventHandler<OnRequestInferenceEventArgs> OnRequestInference;
    public class OnRequestInferenceEventArgs: EventArgs{
        public Texture2D inferenceData;
    }

    // public static EventHandler<OnStageIndexEventArgs> OnStageChange;
    // public class OnStageIndexEventArgs: EventArgs{
    //     public int stageIndex { get; set; }
    //     public bool nextButtonClick { get; set; }
    //     public string stageName { get; set; }
    //     public int functionIndex { get; set; }
    // }

    public static EventHandler<OnBarCodeClickEventArgs> OnBarCodeDetectedEvent;
    public class OnBarCodeClickEventArgs: EventArgs{
        public string barcodeText { get; set; }
    }
}
