using UnityEngine;

public class BirdController : MonoBehaviour {

    #region Properties

    public float mouseVSpeed = 2;
    public float vLimitHigh = -40, vLimitLow = 80;

    public float bottom = -3, top = 2;

    public float baseGravity = 5;
    public float GravityModifier = 0.5f;
    public float fallMultiplier = 2;
    public bool invertAxis;
    public bool ignorePitchUpInput = true;
    public bool isFallingBack;

    float diveModifier;
    public float fallBackStrength = 0.1f;

    float climbing;
    public float maxClimbTime = 1;

    float gravityModifier {
        get {
            return isFallingBack ? Mathf.Abs(vLimitHigh) * diveModifier : Mathf.Abs(pitch) * GravityModifier;
        }
    }

    float posY {
        get {
            return 0;
        }
    }

    public float ySpeed {
        get {
            return -pitch * 0.1f;
        }
    }

    float clampedYSpeed {
        get {
            return posY < bottom ? Mathf.Max(0, ySpeed) : ySpeed;
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
    }

    void Rotate() {
        float value = mouseVSpeed * Input.GetAxis("Mouse Y") / 60;

        if (ignorePitchUpInput)
            value = invertAxis ? Mathf.Max(value, 0) : Mathf.Min(value, 0);

        PreventFlyingUp(ref value);

        pitch = invertAxis ? pitch - value : pitch + value;

        if (pitch < 0 && highestPoint > transform.position.y + top)
            highestPoint = transform.position.y + top;

        ApplyGravity();

        pitch = Mathf.Clamp(pitch, vLimitHigh, vLimitLow);
    }

    void ApplyGravity() {
        gravity = baseGravity + gravityModifier;
        pitch += gravity * Time.deltaTime;
    }

    float fallPitch, fallTime, fallDuration = 1;
    void PreventFlyingUp(ref float value) {
        if (pitch < 0 && transform.position.y > highestPoint)
            climbing += Time.deltaTime;

        if (transform.position.y < highestPoint && isFallingBack) {
            climbing = 0;
            isFallingBack = false;
        }

        if (climbing >= maxClimbTime) {
            if (!isFallingBack) {
                fallPitch = pitch;
                fallTime = 0;
                fallDuration = Mathf.Abs(pitch) * 0.02f;

                diveModifier = Mathf.Abs(pitch * fallBackStrength);
                print(fallDuration);
                isFallingBack = true;
            }

            value = invertAxis ? Mathf.Min(0, value) : Mathf.Max(value, 0);
        }
    }

    #endregion
}
