using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPoseOnDiveValueChanger : MonoBehaviour {

    public BirdPoser poser;

    public float diveValueOnDive = -27;
    public float diveValueOnGlide = 0;

    public float wingBendOnDive = -6.5f;
    public float wingBendOnGlide = 0;


    public float flutterFreqOnDive = 80;
    public float flutterFreqOnGlide = 10;

    public float flutterScaleOnDive = 9;
    public float flutterScaleOnGlide = 3;

    public float flapFreqOnDive = 20;
    public float flapFreqOnGlide = 5;

    public float flapFreqOnNoChange = 5;
    public float flapFreqOnOneChange = 5;

    void Start ()
    {
        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
	}

    float flapTime = 0;

    private void GamePlayController_OnDiveAmountChanged(float v, float d)
    {
        poser.dive = Mathf.Lerp(diveValueOnGlide, diveValueOnDive, v);

        poser.wingBend = Mathf.Lerp(wingBendOnGlide, wingBendOnDive, v);

        if (d > 0)
            flapTime += d * Mathf.InverseLerp(0, 0.7f, v);
        flapTime = Mathf.Min(flapTime, 2);
        poser.flap = Mathf.Clamp01(flapTime);
        flapTime -= Time.deltaTime;

        poser.flapFrequency = Mathf.LerpUnclamped(flapFreqOnNoChange, flapFreqOnOneChange, poser.flap/2);
    }
}
