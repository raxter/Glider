﻿using UnityEngine;

public class BirdController : MonoBehaviour {

    #region Properties
    
    public float mouseVSpeed = 2;
    public float vLimitHigh = -40, vLimitLow = 80;
    float trueVLimit;

    public float top = 2;

    public float baseGravity = 5;
    public float GravityModifier = 0.5f;
    public float upwardSpeedModifier = 0.1f;
    public float maxClimbTime = 0.1f;
    public float fallBackStrength = 0.1f;
    public float tipWeight = 40;

    public bool invertAxis;
    public bool ignorePitchUpInput = true;
    public bool isFallingBack;

    float diveModifier;
    float climbing;
    
    float gravityModifier {
        get {
            return isFallingBack ? Mathf.Abs(vLimitHigh) * diveModifier : (Mathf.Abs(pitch) + tipWeight) * GravityModifier;
        }
    }

    float stabilizer;
    float stabilizationModifier = 0.1f;
    public float ySpeed {
        get {
            return pitch > 0 ? -pitch * 0.1f : -pitch * 0.1f * upwardSpeedModifier;
        }
    }

    float gravity;
    public float pitch { get; set; }

    float highestPoint;

    #endregion

    #region Methods

    void Start() {
        gravity = baseGravity;
        pitch = transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        Rotate();

        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetMouseButtonDown(2))
            invertAxis ^= true;
    }

    void Rotate() {
        if (pitch > -trueVLimit) 
            trueVLimit = Mathf.Max(vLimitHigh, -Mathf.Abs(pitch));
        else if (pitch < 0) 
            trueVLimit = pitch;

        float value = pitch > 0 ? mouseVSpeed * Input.GetAxis("Mouse Y") / 60 : mouseVSpeed * Input.GetAxis("Mouse Y") / 80;

        if (ignorePitchUpInput)
            value = invertAxis ? Mathf.Max(value, 0) : Mathf.Min(value, 0);

        PreventFlyingUp(ref value);

        pitch = invertAxis ? pitch - value : pitch + value;

        if (pitch < 0 && highestPoint > transform.position.y + top)
            highestPoint = transform.position.y + top;

        ApplyGravity();

        pitch = Mathf.Clamp(pitch, trueVLimit, vLimitLow);
    }

    void ApplyGravity() {
        gravity = baseGravity + gravityModifier;
        pitch += gravity * Time.deltaTime;
    }
    
    void PreventFlyingUp(ref float value) {
        if (pitch < 0 && transform.position.y > highestPoint)
            climbing += Time.deltaTime;

        if (transform.position.y < highestPoint && isFallingBack) {
            climbing = 0;
            isFallingBack = false;
        }

        if (climbing >= maxClimbTime) {
            if (!isFallingBack) {
                diveModifier = Mathf.Abs(pitch * fallBackStrength);
                isFallingBack = true;
            }

            value = invertAxis ? Mathf.Min(0, value) : Mathf.Max(value, 0);
        }
    }

    #endregion

}
