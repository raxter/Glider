using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextAfterSeconds : MonoBehaviour
{

    public float delay = 5;
    public float fade = 1;

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(delay);

        GetComponent<Text>().CrossFadeAlpha(0, fade, false);
	}
	
}
