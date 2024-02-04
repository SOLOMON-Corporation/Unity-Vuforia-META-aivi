using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Text.RegularExpressions;
public class BarcodeInteraction : MonoBehaviour
{
    BarcodeBehaviour mBarcodeBehaviour;
    public Button metaButton;
    public Button fiixButton;
    public Button nextButton;
    private string scannedBarcode;
    private ColorBlock colorBlockGreen;
    private ColorBlock colorBlockWhite;
    private string port = "5000";
    [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>(); 
        colorBlockGreen = metaButton.colors;
        colorBlockGreen.normalColor = Color.green;
        colorBlockWhite = metaButton.colors;
        colorBlockWhite.normalColor = Color.white;
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
            return;
        }
        //OnBarCodeDetectedHandler();
        if (StationStageIndex.barcodeMetaOn) 
        {
            StationStageIndex.FunctionIndex = "VuforiaTargetDetecting";
            return;
        }
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)
        {
            if (scannedBarcode != mBarcodeBehaviour.InstanceData.Text){
                EventManager.OnBarCodeDetectedEvent?.Invoke(this, new EventManager.OnBarCodeClickEventArgs{
                    barcodeText = mBarcodeBehaviour.InstanceData.Text
                });
                Debug.Log(mBarcodeBehaviour.transform.position);
            }
        }
        if (StationStageIndex.barcodeMetaOn){ 
            uiMessage.text = "Scan META success";
            metaButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
        }
        else{
            metaButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        if (StationStageIndex.barcodeFiixOn){
            fiixButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
        }
        else{
            fiixButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
    }

    private void OnBarCodeDetectedHandler(object sender, EventManager.OnBarCodeClickEventArgs e)
    {
        string[] barcodeStringArray = Regex.Replace(e.barcodeText, @"[\'\""\[\]\\\s]+", "")
                .Split(new[] { ',' });
        if (barcodeStringArray.Length > 4)
        {
            if (!StationStageIndex.barcodeMetaOn){
                MetaService.qrMetaData = barcodeStringArray;
                StationStageIndex.barcodeMetaOn = true;
                nextButton.gameObject.SetActive(true);
                // Set Config
                if (ConfigRead.metaOnline){
                    MetaService.serverIP = MetaService.qrMetaData[0] + ":" + port;
                    MetaService.ConnectWithMetaProjectID();
                }
            }
        }
        else
        {
            StationStageIndex.barcodeFiixOn = true;
        }
    }
    private void OnBarCodeDetectedHandler()
    {
        string[] barcodeStringArray= new string[]
                    {
                        "10.1.30.18",
                        "MTIzMTIz",
                        "PTC_validation_scenario",
                        "1688970534",
                        "demo"
                    };
        //"1688627566"
        if (!StationStageIndex.barcodeMetaOn)
        {
            MetaService.qrMetaData = barcodeStringArray;
            StationStageIndex.barcodeMetaOn = true;
            nextButton.gameObject.SetActive(true);
            // Set Config
            if (ConfigRead.metaOnline)
            {
                MetaService.serverIP = MetaService.qrMetaData[0] + ":" + port;
                MetaService.ConnectWithMetaProjectID();
            }
        }
    }
}
