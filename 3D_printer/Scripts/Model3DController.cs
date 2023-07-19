using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model3DController : MonoBehaviour
{
    private bool isEventAdded = false;
    private int itemID = 0;
    private Transform[] childTransforms;
    private Transform[] itemTransforms;
    public GameObject midAirPositioner;
    // public Button nextButton;
    // public NextButtonClickHandler nextButtonClickHandler;
    // [SerializeField]  private TMPro.TextMeshProUGUI uiMessage;
    void Start()
    {
        childTransforms = gameObject.transform.GetComponentsInChildren<Transform>();
        if (!isEventAdded)
        {
            EventManager.OnModelChangeButtonClick += ButtonClick;
            isEventAdded = true;
        }
        RaiseButtonClick();
        //midAirPositionGB.SetActive(false);
    }
    void OnDisable()
    {
        if (isEventAdded)
        {
            EventManager.OnModelChangeButtonClick -= ButtonClick;
            isEventAdded = false;
        }
    }
    void OnEnable()
    {
        if (!isEventAdded)
        {
            EventManager.OnModelChangeButtonClick += ButtonClick;
            isEventAdded = true;
        }
    }
    private void RaiseButtonClick()
    {
        int count = 0;
        foreach (Transform childTran in childTransforms)
        {
            if (childTran.name.Contains("Item"))
            {
                count++;
            }
        }
        itemTransforms = new Transform[count];
        count = 0;
        bool displayedItem = false;
        foreach (Transform childTran in childTransforms)
        {
            if (childTran.name.Contains("Item"))
            {
                itemTransforms[count] = childTran;
                count++;
                if (!displayedItem){
                    childTran.gameObject.SetActive(true);
                    displayedItem = true;
                }
                else{
                    childTran.gameObject.SetActive(false);
                }
            }
        }
    }
    private void ButtonClick(object sender, EventManager.OnModelChangeButtonClickEventArgs e)
    {
        if (midAirPositioner.activeSelf)
        {
            midAirPositioner.SetActive(false);
        }
        if (e.buttonType)
        {
            if (itemID == itemTransforms.Length - 1)
            {
                itemID = 0;
            }
            else
            {
                itemID += 1;
            }
        }
        else
        {
            if (itemID == 0)
            {
                itemID = itemTransforms.Length - 1;
            }
            else
            {
                itemID -= 1;
            }
        }
        Change3DItem();
    }
    private void Change3DItem()
    {
        for (int index = 0; index < itemTransforms.Length; index++)
        {
            if (index == itemID)
            {
                itemTransforms[index].gameObject.SetActive(true);
            }
            else
            {
                itemTransforms[index].gameObject.SetActive(false);
            }
        }
    }
}
