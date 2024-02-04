using UnityEngine;
using System;

public static class EventManager
{
    public static EventHandler<OnStageIndexEventArgs> OnStageChange;
    public static EventHandler<OnRequestInferenceEventArgs> OnRequestInference;
    public static EventHandler<OnBarCodeClickEventArgs> OnBarCodeDetectedEvent;
    public static EventHandler<OnModelChangeButtonClickEventArgs> OnModelChangeButtonClick;

    // Event arguments for stage change event
    public class OnStageIndexEventArgs : EventArgs
    {
        public int stageIndex { get; set; }
        public bool nextButtonClick { get; set; }
        public string stageName { get; set; }
        public int functionIndex { get; set; }
    }

    // Event arguments for request inference event
    public class OnRequestInferenceEventArgs : EventArgs
    {
        public Texture2D inferenceData;
    }

    // Event arguments for barcode detected event
    public class OnBarCodeClickEventArgs : EventArgs
    {
        public string barcodeText { get; set; }
    }

    // Event arguments for model change button click event
    public class OnModelChangeButtonClickEventArgs : EventArgs
    {
        public bool buttonType { get; set; }
    }
}
