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
    public float[] trackDiveCutOffFreq;
    public int nextPlayingTrackNum = 1;

    IEnumerator trackBlending()
    {
        float currentVolume;
        float nextVolume;

        float blendingStep = 0;

        do
        {
            blendingStep = currentFlightSpeed / 20f;
            blendingStep = Mathf.Clamp(blendingStep, 0.3f, 1f);

            audiomixer.GetFloat("currentTrackVolume", out currentVolume);
            audiomixer.SetFloat("currentTrackVolume", currentVolume - blendingStep);

            audiomixer.GetFloat("nextTrackVolume", out nextVolume);
            audiomixer.SetFloat("nextTrackVolume", Mathf.Min(nextVolume + blendingStep * 2, 0f));

            //average of current real cutoff and the preset cutoff of the next track to blend slowly there
            GetComponent<drop_and_glide_blend>().currentTrackCutOffFreq = ((GetComponent<drop_and_glide_blend>().currentTrackCutOffFreq * 10) + trackDiveCutOffFreq[nextPlayingTrackNum]) / 11;

            yield return new WaitForSeconds(0.1f);
        }
        while (nextVolume < 0f);

        currentSource.time %= nextSource.clip.length;
        currentSource.clip = nextSource.clip;
        currentSource.Play();

        GetComponent<drop_and_glide_blend>().currentTrackCutOffFreq = trackDiveCutOffFreq[nextPlayingTrackNum];

        currentSource.time = nextSource.time;

        nextPlayingTrackNum++;

        if (nextPlayingTrackNum >= trackList.Length)
        {
            nextPlayingTrackNum = 0;
        }

        nextSource.clip = trackList[nextPlayingTrackNum];
        nextSource.Play();

        audiomixer.SetFloat("currentTrackVolume", 0f);
        audiomixer.SetFloat("nextTrackVolume", -40f);

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

        if (currentTrackProgress > 200f)
        {
            nextTrack();
            currentTrackProgress = 0f;
        }

    }
}
