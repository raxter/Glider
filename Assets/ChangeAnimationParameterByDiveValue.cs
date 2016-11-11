using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationParameterByDiveValue : MonoBehaviour {

    public string parameterName;
    public float min = 0;
    public float max = 1;
    // Use this for initialization
    void Start () {
        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
	}

    private void GamePlayController_OnDiveAmountChanged(float v)
    {
        GetComponent<Animator>().SetFloat(parameterName, Mathf.Lerp(min, max, v));
    }
    
}
