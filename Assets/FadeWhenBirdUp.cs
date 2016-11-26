using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeWhenBirdUp : MonoBehaviour
{
    public BirdController bird;

    public float fadeAfterYSpeed = -1;
    public float fadeInDelay = 2;
    public float fade = 1;

    bool faded = false;

    IEnumerator Start()
    {
        faded = true;
        GetComponent<Text>().CrossFadeAlpha(0, 0, false);
        yield return new WaitForSeconds(fadeInDelay);

        GetComponent<Text>().CrossFadeAlpha(1, fade, false);

        yield return new WaitForSeconds(fade);

        faded = false;
    }

    void Update ()
    {
        if (!faded && bird.ySpeed > fadeAfterYSpeed)
        {
            faded = true;
            GetComponent<Text>().CrossFadeAlpha(0, 1, false);
        }
	}
}
