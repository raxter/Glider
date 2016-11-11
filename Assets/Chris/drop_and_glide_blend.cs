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

        if (bird != null)
            bird.rotation = Quaternion.Euler(0f, 0f, newValue * 90f);

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
