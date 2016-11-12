using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFOVByDiveAmount : MonoBehaviour {

    public float diveFOV = 50;
    public float glideFOV = 75;
	// Use this for initialization
	void Start () {
        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
	}

    private void GamePlayController_OnDiveAmountChanged(float v, float d)
    {
        // TODO lerp
        Camera c = GetComponent<Camera>();
        float targetFOV = Mathf.Lerp(glideFOV, diveFOV, v);
        c.fieldOfView = Mathf.Lerp(c.fieldOfView, targetFOV, 3*Time.deltaTime);
    }
    
}
