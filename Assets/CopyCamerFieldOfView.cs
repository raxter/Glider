using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCamerFieldOfView : MonoBehaviour
{
    public Camera targetCamera;    
	
	// Update is called once per frame
	void Update () {
        GetComponent<Camera>().fieldOfView = targetCamera.fieldOfView;
    }
}
