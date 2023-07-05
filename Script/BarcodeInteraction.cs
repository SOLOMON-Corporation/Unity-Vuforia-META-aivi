using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
public class BarcodeInteraction : MonoBehaviour
{
    public GameObject barcodeObject;
    BarcodeBehaviour mBarcodeBehaviour;
    public Button metaButton;
    public Button fiixButton;
    public Button nextButton;
    private string scannedBarcode;
    private ColorBlock colorBlockGreen;
    private ColorBlock colorBlockWhite;
    
    private string[] qrFiixData;
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    // public MetaAPI metaAPI;
    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>(); 
        colorBlockGreen = metaButton.colors;
        colorBlockGreen.normalColor = Color.green;
        colorBlockWhite = metaButton.colors;
        colorBlockWhite.normalColor = Color.white;
        EventManager.OnBarCodeDetectedEvent += OnBarCodeDetectedHandler;
    }

    void OnDisable(){
        EventManager.OnBarCodeDetectedEvent -= OnBarCodeDetectedHandler;
    }

    void OnEnable(){
        EventManager.OnBarCodeDetectedEvent += OnBarCodeDetectedHandler;
    }

    void Update()
    {
        if (StationStageIndex.FunctionIndex != "ScanBarcode"){
            uiMessage.text = "Scan META";
            StationStageIndex.FunctionIndex = "ScanBarcode";
        }
        if (StationStageIndex.barcodeMetaOn) //&& StationStageIndex.barcodeFiixOn)
        {
            // barcodeObject.SetActive(false);
            StationStageIndex.FunctionIndex = "VuforiaTarget";
            //MetaApiStatic.ConnectMetaBasedProjectID(1678700647);
            return;
        }
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)// && StationStageIndex.FunctionIndex == "ScanBarcode")// && !ARCameraScript.barcodeMetaOn)
        {
            // if (uiMessage.text != mBarcodeBehaviour.InstanceData.Text){
            //     uiMessage.text = "Click barcode to choose project";//mBarcodeBehaviour.InstanceData.Text;
            // }
            if (scannedBarcode != mBarcodeBehaviour.InstanceData.Text){
                EventManager.OnBarCodeDetectedEvent?.Invoke(this, new EventManager.OnBarCodeClickEventArgs{
                    barcodeText = mBarcodeBehaviour.InstanceData.Text
                });
                Debug.Log(mBarcodeBehaviour.transform.position);
            }
        }
        if (StationStageIndex.barcodeMetaOn){
            uiMessage.text = "Scan META success";
            metaButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;//.colors = colorBlockGreen;
        }
        else{
            metaButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;//.colors = colorBlockWhite;
        }
        if (StationStageIndex.barcodeFiixOn){
            uiMessage.text = "Scan Fiix success";
            fiixButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;//.colors = colorBlockGreen;
        }
        else{
            fiixButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;//.colors = colorBlockWhite;
        }
        // else{
        //     // barcodeAsText.text = "";
        //     return;
        // }
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //     if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        //     {
        //         selectedBarcode = hit.collider.GetComponent<BarcodeBehaviour>().InstanceData.Text;
        //         //Raise event here
        //         EventManager.OnBarCodeClickEvent?.Invoke(this, new EventManager.OnBarCodeClickEventArgs{
        //             barcodeText = selectedBarcode
        //         });
        //         Debug.Log(selectedBarcode);
        //         // Can raise event in here ==> call meta API
        //     }
        // }
    }

    private void OnBarCodeDetectedHandler(object sender, EventManager.OnBarCodeClickEventArgs e)
    {
        // Debug.Log("START OnBarCodeClickHandler");
        // if (metaAPI.triggerAPIresponseData.requestResult && imageTargetFound)
        // {
        //     Debug.Log("StartCoroutine");
        //     uiMessage.text = "";
        //     // CallMetaAPIinference();
        // }
        // uiMessage.text = "Move camera close to target";
        // string barcodeString;
        string[] barcodeStringArray;
        // if (!e.barcodeText.Contains("[")){
        //     barcodeString = DecryptionRSA.DecrypteBarCode(e.barcodeText);
        // }
        barcodeStringArray = e.barcodeText.Replace("\'", "").Replace("\"b", "").Replace(" ", "").Replace("\\", "").Trim('[', ']').Split(new[] { ',' }).Select(x => x.Trim('"')).ToArray();//
        if (barcodeStringArray.Length > 4)//(barcodeJsonString.Contains("project_id"))
        {
            if (!StationStageIndex.barcodeMetaOn){
                MetaApiStatic.qrMetaData = barcodeStringArray;
                //barcodeJsonString = barcodeJsonString.Replace("None", "''");
                //var jsonData = JsonConvert.DeserializeObject<RSAbarDecreptionObject>(barcodeJsonString);
                // Set MetaAPI
                //metaAPI.query_ip = jsonData.server_ip + ":5000";// hard code
                //metaAPI.ConnectMetaBasedProjectID(jsonData.project_id);
                StationStageIndex.barcodeMetaOn = true;
                nextButton.gameObject.SetActive(true);
                // Set Config
                MetaApiStatic.query_ip = MetaApiStatic.qrMetaData[0] + ":5000";
                MetaApiStatic.ConnectMetaBasedProjectID();
                // string projectFile = $"Project/{MetaApiStatic.qrMetaData[2]}.json";
                // StartCoroutine(ConfigRead.LoadJSONFileProject(Path.Combine(Application.streamingAssetsPath,projectFile)));
                // StartCoroutine(ConfigRead.LoadJSONFileConfig(Path.Combine(Application.streamingAssetsPath, "Config/InputParameter.json")));
            }
        }
        else //if (barcodeJsonString.Contains("accessKey"))
        {
            qrFiixData = barcodeStringArray;
            // var jsonData = JsonConvert.DeserializeObject<RSAfiixDecreptionObject>(barcodeJsonString);
            // var fixxData = 
            StationStageIndex.barcodeFiixOn = true;
            Debug.Log("Got Fiix");
            // Set config Fiix
        }
    }

}
