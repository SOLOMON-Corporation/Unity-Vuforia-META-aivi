using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenShortClickHandler : MonoBehaviour
{
    public Button screenshotButton;
    // Start is called before the first frame update
    void Start()
    {
        screenshotButton.onClick.AddListener(RaiseButtonClick);
    }

    private void RaiseButtonClick(){
        ImageProcessing.TakeScreenshot();
    }
}
