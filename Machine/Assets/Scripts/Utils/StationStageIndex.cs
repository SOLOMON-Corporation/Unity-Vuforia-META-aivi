using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class StationStageIndex
{
    // Indicates whether barcode meta is on or off
    public static bool barcodeMetaOn = false;

    // Indicates whether barcode Fiix is on or off
    public static bool barcodeFiixOn = false;

    // Determines the meta inference rule
    public static bool metaInferenceRule;

    // The index of the current stage
    public static int stageIndex;

    // The name of the current stage
    public static string stageName;

    // The index of the current station
    public static int stationIndex = 0;

    // The size of the mesh renderer bounds for the current stage
    public static Vector3 stageMeshRenderBoundSize;

    // The position of the current stage
    public static Vector3 stagePosition;

    // Dictionary that maps game object names to their corresponding points
    public static Dictionary<string, Vector3[]> gameObjectPoints = new Dictionary<string, Vector3[]>();

    // The X margin data for the current stage
    public static float marginXdata;

    // The Y margin data for the current stage
    public static float marginYdata;

    // List of available functions
    public static string[] functionList = { "Home", "ScanBarcode", "VuforiaTargetDetecting", "VuforiaTarget", "Sample", "Detect", "Result" };

    // Stopwatch used for measuring meta time count
    public static Stopwatch metaTimeCount;

    // The total minutes for meta time count
    public static float metaTotalMinute;

    // The total seconds for meta time count
    public static float metaTotalSecond;

    // The temporary minutes for meta time count
    public static float metaTempMinute;

    // The temporary seconds for meta time count
    public static float metaTempSecond;

    // Updates the margin values based on the provided data stage
    public static void UpdateMargin(Datastage dataStage)
    {
        marginXdata = (float)dataStage.Agrs.MarginX;
        marginYdata = (float)dataStage.Agrs.MarginY;
    }

    // The current function index
    private static string functionIndex = "_";

    // Event triggered when the function index changes
    public static event System.Action<string> OnFunctionIndexChange;

    // Property for accessing and updating the function index
    public static string FunctionIndex
    {
        get { return functionIndex; }
        set
        {
            if (value != functionIndex)
            {
                functionIndex = value;
                OnFunctionIndexChange?.Invoke(functionIndex);
            }
        }
    }

    // Indicates whether an image target has been found
    public static bool imageTargetFound;

    // Event triggered when the image target found status changes
    public static event System.Action<bool> OnImageTargetFoundChange;

    // Property for accessing and updating the image target found status
    public static bool ImageTargetFound
    {
        get { return imageTargetFound; }
        set
        {
            if (value != imageTargetFound)
            {
                imageTargetFound = value;
                OnImageTargetFoundChange?.Invoke(imageTargetFound);
            }
        }
    }

    // Indicates whether the final UI is being displayed
    public static bool finalUI;

    // Event triggered when the final UI status changes
    public static event System.Action<bool> OnFinalUIChange;

    // Property for accessing and updating the final UI status
    public static bool FinalUI
    {
        get { return finalUI; }
        set
        {
            if (value != finalUI)
            {
                finalUI = value;
                OnFinalUIChange?.Invoke(finalUI);
            }
        }
    }
}
