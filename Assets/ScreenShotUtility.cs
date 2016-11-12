using UnityEngine;
using System.Collections;

public class ScreenShotUtility : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            TakeScreen();
        }

    }

    void TakeScreen()
    {
        string filename = System.DateTime.Now.ToString("yyyy-MM-dd_HHmm_ss");
        Debug.Log(filename);
        Application.CaptureScreenshot(filename + ".png", 1);
        Application.CaptureScreenshot(filename + "_x2.png", 2);
    }
}
