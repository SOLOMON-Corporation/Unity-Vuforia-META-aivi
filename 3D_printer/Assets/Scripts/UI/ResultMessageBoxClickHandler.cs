using UnityEngine;
public class ResultMessageBoxClickHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public float expansionRate = 0.8f;
    private Vector2 initialSize;
    private Vector2 targetSize;
    private bool isExpanding;
    private bool isMaxSize;
    private RectTransform targetRect;
    [SerializeField]  private TMPro.TextMeshProUGUI ResultDetail;
    [SerializeField]  private TMPro.TextMeshProUGUI expandButtonSymble;
    void Start()
    {
        initialSize = canvas.GetComponent<RectTransform>().sizeDelta;
        targetSize = initialSize * 2.0f;
        targetRect= canvas.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isExpanding)
        {
            // Convert the touch position to local space
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, Input.GetTouch(0).position, null, out localPoint);

            // Check if the touch position is within the target rect transform
            if (targetRect.rect.Contains(localPoint))
            {
                isExpanding = true;
            }
        }
        if (isExpanding)
        {
            if (!isMaxSize){
                targetRect.sizeDelta += new Vector2 (0,targetSize[1]) * expansionRate * Time.deltaTime;
                transform.position += new Vector3 (0,targetSize[1] * expansionRate * Time.deltaTime*0.5f,0);
                if (targetRect.sizeDelta[1] >= targetSize[1])
                {
                    isMaxSize = true;
                    isExpanding = false;
                    expandButtonSymble.text = "˅";
                }
            }
            else{
                targetRect.sizeDelta -= new Vector2 (0,targetSize[1]) * expansionRate * Time.deltaTime;
                transform.position -= new Vector3 (0,targetSize[1] * expansionRate * Time.deltaTime*0.5f,0);
                if (targetRect.sizeDelta[1] <= initialSize[1])
                {
                    isMaxSize = false;
                    isExpanding = false;
                    expandButtonSymble.text = "˄";
                }
            }
        }
        if (isMaxSize && targetRect.sizeDelta[1] >= targetSize[1]){
            ResultDetail.text = $"Total Elapsed Time: {StationStageIndex.metaTotalMinute}:{StationStageIndex.metaTotalSecond} \n\n Current Check point elapsed time: {StationStageIndex.metaTempMinute}:{StationStageIndex.metaTempSecond}";
        }
        else{
            ResultDetail.text = "";
        }
    }
}
