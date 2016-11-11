using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;


public class TrackBlender : MonoBehaviour
{
    public AudioMixer audiomixer;
    public AudioSource currentSource;
    public AudioSource nextSource;
    public Text currentTrackProgressTextObject;

    public float currentFlightSpeed = 0f;

    float currentTrackProgress = 0f;

    public AudioClip[] trackList;

    void nextTrack()
    {

    }


    // Use this for initialization
    void Start()
    {
        currentSource.clip = trackList[0];
        nextSource.clip = trackList[1];
        currentSource.Play();
        nextSource.Play();

        audiomixer.SetFloat("currentTrackVolume", 0f);
        audiomixer.SetFloat("nextTrackVolume", -80f);

    }

    // Update is called once per frame
    void Update()
    {
        currentTrackProgress += currentFlightSpeed * Time.deltaTime;

        if (currentTrackProgressTextObject != null)
            currentTrackProgressTextObject.text = "" + currentTrackProgress;

        if (currentTrackProgress > 100f)
        {
            nextTrack();
            currentTrackProgress = 0f;
        }

    }
}
