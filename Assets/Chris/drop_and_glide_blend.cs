using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class drop_and_glide_blend : MonoBehaviour
{
    public AudioMixer audiomixer;
    public Transform bird;

    public Transform trackBlender;

    // Use this for initialization
    void Start()
    {
        adjustGlideAndDropBlend(0f);
    }

    public void adjustGlideAndDropBlend(float newValue)
    {
        bird.rotation = Quaternion.Euler(0f, 0f, newValue * 90f);
        audiomixer.SetFloat("swooshEQGain", newValue);
        audiomixer.SetFloat("musicEQGain", 1f - newValue);
        audiomixer.SetFloat("swooshVolume", -10f + newValue * 10f);

        trackBlender.GetComponent<TrackBlender>().currentFlightSpeed = newValue * 30f;

    }



    // Update is called once per frame
    void Update()
    {

    }
}
