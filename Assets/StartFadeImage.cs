using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFadeImage : MonoBehaviour {

    public float fade = 1;

	// Use this for initialization
	IEnumerator Start ()
    {
        Image image = GetComponent<Image>();
        image.color = Color.white;
        yield return new WaitForSeconds(fade);
	}
	
}
