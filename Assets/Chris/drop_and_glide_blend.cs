using UnityEngine;
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
        audiomixer.SetFloat("musicVolume", -4f + (1f - newValue) * 4f);
        audiomixer.SetFloat("musicCutoffFreq", 22000f - newValue * 19000f);
        audiomixer.SetFloat("swooshVolume", -11f + swooshSoundVolume.Evaluate(newValue) * 10f);

        trackBlender.GetComponent<TrackBlender>().currentFlightSpeed = newValue * 30f;

        float currentSpeed = trackBlender.GetComponent<TrackBlender>().currentFlightSpeed;
        float speedDelta = oldFlightSpeed - currentSpeed;
        if (speedDelta > 0.5f && currentSpeed < 10f)
        {
            if (!suddenSwoosh.GetComponent<AudioSource>().isPlaying)
            {
                suddenSwoosh.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.15f);
                suddenSwoosh.GetComponent<AudioSource>().volume = Mathf.Lerp(0.2f, 1, Mathf.Clamp01((speedDelta - 0.5f) / 2));
                suddenSwoosh.GetComponent<AudioSource>().Play();
            }

        }

        oldFlightSpeed = currentSpeed;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
