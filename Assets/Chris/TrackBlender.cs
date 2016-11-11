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

    IEnumerator trackBlending()
    {
        float currentVolume;
        float nextVolume;

        do
        {
            audiomixer.GetFloat("currentTrackVolume", out currentVolume);
            audiomixer.SetFloat("currentTrackVolume", currentVolume - currentFlightSpeed / 10f);

            audiomixer.GetFloat("nextTrackVolume", out nextVolume);
            audiomixer.SetFloat("nextTrackVolume", nextVolume + currentFlightSpeed / 10f);

            yield return new WaitForSeconds(0.1f);
        }
        while (nextVolume < 0f);

        audiomixer.SetFloat("nextTrackVolume", 0f);

    }

    void nextTrack()
    {
        StartCoroutine(trackBlending());

    }

    // Use this for initialization
    void Start()
    {
        currentSource.clip = trackList[0];
        nextSource.clip = trackList[1];
        currentSource.Play();
        nextSource.Play();

        audiomixer.SetFloat("currentTrackVolume", 0f);
        audiomixer.SetFloat("nextTrackVolume", -40f);

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
