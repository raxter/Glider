﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class drop_and_glide_blend : MonoBehaviour
{
    public AudioMixer audiomixer;

    private Transform suddenSwoosh;

    public AnimationCurve swooshSoundVolume;

    public Transform trackBlender;

    private float oldFlightSpeed = 0;

    public float GlideToDropBlend
    {
        set
        {
            adjustGlideAndDropBlend(value);
        }
    }

    // Use this for initialization
    void Start()
    {
        suddenSwoosh = transform.FindChild("SuddenSwoosh");
        adjustGlideAndDropBlend(0f);
    }

    public void adjustGlideAndDropBlend(float newValue)
    {
        newValue = Mathf.Clamp01(newValue);

        audiomixer.SetFloat("swooshEQGain", newValue);
        audiomixer.SetFloat("musicVolume", -5f + (1f - newValue) * 5f);
        audiomixer.SetFloat("swooshVolume", -10f + swooshSoundVolume.Evaluate(newValue) * 10f);

        trackBlender.GetComponent<TrackBlender>().currentFlightSpeed = newValue * 30f;

        if (oldFlightSpeed - trackBlender.GetComponent<TrackBlender>().currentFlightSpeed > 3f && trackBlender.GetComponent<TrackBlender>().currentFlightSpeed < 10f)
        {
            if (!suddenSwoosh.GetComponent<AudioSource>().isPlaying)
            {
                suddenSwoosh.GetComponent<AudioSource>().Play();
            }

        }

        oldFlightSpeed = trackBlender.GetComponent<TrackBlender>().currentFlightSpeed;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
