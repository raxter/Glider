using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GamePlayController : MonoBehaviour
{
    [Range(-1, 1)]
    public float diveAmount = 0;
    public float pitchAmount = 0;

    public AnimationCurve birdDip;
    public AnimationCurve birdDepth;
    public AnimationCurve cameraDip;
    public Transform bird;
    public Camera mainCamera;

    public SkyboxController skyboxController;

    public BirdController birdController;

    // Update is called once per frame
    void Update ()
    {
        if (Application.isPlaying)
        {
            pitchAmount = birdController.pitch;
            diveAmount = birdController.pitch;
            diveAmount /= diveAmount >= 0 ? birdController.vLimitLow : -birdController.vLimitHigh;
        }

        UpdateDiveAmount();
        
        if (Application.isPlaying)
        {
            birdController.transform.localPosition += Vector3.up * birdController.ySpeed * Time.deltaTime;
        }
        skyboxController.Depth = -birdController.transform.position.y;

    }

    void UpdateDiveAmount()
    {
        bird.transform.localEulerAngles = Vector3.right * pitchAmount;

        //mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.Euler(Vector3.right * cameraDip.Evaluate(diveAmount)), 10*Time.deltaTime);

        //bird.transform.localEulerAngles = Vector3.right * birdDip.Evaluate(diveAmount);
        //bird.transform.localPosition = Vector3.down * birdDepth.Evaluate(diveAmount);
    }
}
