using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class StationStageIndex
{
    public static bool barcodeMetaOn = false;
    public static bool barcodeFiixOn = false;
    public static bool metaInferenceRule;
    public static int stageIndex;// = 0;
    public static string stageName;
    public static float startTime;
    public static int stationIndex = 1;
    public static Vector3 stageMeshRenderBoundSize;
    public static Vector3 stagePosition;
    public static float marginXdata;
    public static float marginYdata;
    public static int[] metaAPIimageResize = {640,480};
    public static float foreground2backgroundRatio = 0.5f;
    public static string[] functionList = {"Home","ScanBarcode","VuforiaTarget","Sample","Detect","Result"};
    public static void UpdateMargin(Datastage dataStage){
        marginXdata = (float)dataStage.Agrs.MarginX; //increase percentage
        marginYdata = (float)dataStage.Agrs.MarginY;
    }
    private static string functionIndex = "Home";
    public static event System.Action<string> OnFunctionIndexChange;
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
    public static bool imageTargetFound;
    public static event System.Action<bool> OnImageTargetFoundChange;
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
    public static List<Vector2[]>  imageTargetGameObjectPositions;
    public static event System.Action<List<Vector2[]>> OnImageTargetGameObjectPositionsChange;
    public static List<Vector2[]> ImageTargetGameObjectPositions
    {
        get { return imageTargetGameObjectPositions; }
        set
        {
            if (value != imageTargetGameObjectPositions)
            {
                imageTargetGameObjectPositions = value;
                OnImageTargetGameObjectPositionsChange?.Invoke(imageTargetGameObjectPositions);
            }
        }
    }
}