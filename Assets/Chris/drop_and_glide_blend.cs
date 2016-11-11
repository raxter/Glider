using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class drop_and_glide_blend : MonoBehaviour
{
    public AudioMixer audiomixer;
    public Transform bird;

    public AnimationCurve swooshSoundVolume;

    public Transform trackBlender;

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
        adjustGlideAndDropBlend(0f);
    }

    public void adjustGlideAndDropBlend(float newValue)
    {
        newValue = Mathf.Clamp01(newValue);

        audiomixer.SetFloat("swooshEQGain", newValue);
        audiomixer.SetFloat("musicEQGain", 1f - newValue);
        audiomixer.SetFloat("swooshVolume", -10f + swooshSoundVolume.Evaluate(newValue) * 10f);

        trackBlender.GetComponent<TrackBlender>().currentFlightSpeed = newValue * 30f;

    }



    // Update is called once per frame
    void Update()
    {

    }
}
