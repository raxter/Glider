using UnityEngine;
using UnityEngine.Audio;

public class drop_and_glide_blend : MonoBehaviour
{
    public AudioMixer audiomixer;

    private Transform suddenSwoosh;

    public float currentTrackCutOffFreq = 3000f;

    public AnimationCurve swooshSoundVolume;
    public float swooshSpeed = 25;
    public float swooshAngleDelta = 5;

    public Transform trackBlender;

    private float oldFlightSpeed = 0;
    private bool canSwoosh;

    public float GlideToDropBlend
    {
        set
        {
            adjustGlideAndDropBlend(value);
        }
    }
    
    void Start()
    {
        currentTrackCutOffFreq = GetComponent<TrackBlender>().trackDiveCutOffFreq[0];
        suddenSwoosh = transform.FindChild("SuddenSwoosh");
        adjustGlideAndDropBlend(0f);
    }

    public void adjustGlideAndDropBlend(float newValue)
    {
        newValue = Mathf.Clamp01(newValue);

        audiomixer.SetFloat("swooshEQGain", newValue);
        audiomixer.SetFloat("musicVolume", -4f + (1f - newValue) * 4f);
        audiomixer.SetFloat("musicCutoffFreq", currentTrackCutOffFreq + 19000f - newValue * 19000f);
        audiomixer.SetFloat("swooshVolume", -11f + swooshSoundVolume.Evaluate(newValue) * 10f);

        trackBlender.GetComponent<TrackBlender>().currentFlightSpeed = newValue * 30f;

        float currentSpeed = trackBlender.GetComponent<TrackBlender>().currentFlightSpeed;
        if (currentSpeed > swooshSpeed) canSwoosh = true;
        float speedDelta = oldFlightSpeed - currentSpeed;
        if (canSwoosh && speedDelta > swooshAngleDelta && currentSpeed < 10f)
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
}
