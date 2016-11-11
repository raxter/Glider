using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GamePlayController : MonoBehaviour
{
    [Header("DEBUG STUFF")]
    public bool overrideDiveValue = false;
 [Header("NORMAL STUFF")]
    [Range(-1, 1)]
    public float diveAmount = 0;
    public float pitchAmount = 0;

    public AnimationCurve birdDip;
    public AnimationCurve birdDepth;
    public AnimationCurve cameraDip;
    public Transform bird;
    public Camera mainCamera;

    public SkyboxController skyboxController;

    public drop_and_glide_blend dropAndGlideBlender;

    public BirdController birdController;


    float oldDiveAmount = 0;
    public static event System.Action<float, float> OnDiveAmountChanged;
    // Update is called once per frame
    void Update ()
    {
        if (Application.isPlaying)
        {
            if (!overrideDiveValue)
            {
                pitchAmount = birdController.pitch;
                diveAmount = birdController.pitch;
                diveAmount /= diveAmount >= 0 ? birdController.vLimitLow : -birdController.vLimitHigh;
            }
            else
            {
                // update pitch for sanity
                if (diveAmount >= 0)
                    pitchAmount = Mathf.Lerp(0, birdController.vLimitLow, diveAmount);
                else
                    pitchAmount = Mathf.Lerp(0, birdController.vLimitHigh, - diveAmount);

                birdController.pitch = pitchAmount;
            }
            if (OnDiveAmountChanged != null)
                OnDiveAmountChanged(diveAmount, (oldDiveAmount - diveAmount) / Time.deltaTime);
            
        }

        UpdateDiveAmount();
        
        if (Application.isPlaying)
        {
            birdController.transform.localPosition += Vector3.up * birdController.ySpeed * Time.deltaTime;

            dropAndGlideBlender.GlideToDropBlend = diveAmount;
        }
        skyboxController.Depth = -birdController.transform.position.y;


        oldDiveAmount = diveAmount;
    }

    void UpdateDiveAmount()
    {
        bird.transform.localEulerAngles = Vector3.right * pitchAmount;

        //mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.Euler(Vector3.right * cameraDip.Evaluate(diveAmount)), 10*Time.deltaTime);

        //bird.transform.localEulerAngles = Vector3.right * birdDip.Evaluate(diveAmount);
        //bird.transform.localPosition = Vector3.down * birdDepth.Evaluate(diveAmount);
    }
}
