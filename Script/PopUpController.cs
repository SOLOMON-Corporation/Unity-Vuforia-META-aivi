using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    public GameObject popUpPanel;
    public Text messageText;
    public Button confirmButton;
    public Button cancelButton;

    public void ShowPopUp(string message, System.Action onConfirm, System.Action onCancel)
    {
        messageText.text = message;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            ClosePopUp();
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            onCancel?.Invoke();
            ClosePopUp();
        });

        popUpPanel.SetActive(true);
    }

    public void ClosePopUp()
    {
        popUpPanel.SetActive(false);
    }
}
