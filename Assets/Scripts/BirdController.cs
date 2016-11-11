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
    public bool isFallingBack;

    float diveModifier;
    public float fallBackStrength = 0.1f;

    float gravityModifier {
        get {
            if (posY < 0) diveModifier = GravityModifier;
            return posY > 0 ? diveModifier : GravityModifier;
        }
    }

    float posY
    {
        get
        {
            return 0;// transform.localPosition.y;
        }
    }

    public float ySpeed {
        get {
            return -pitch * 0.1f;
        }
    }

    //float cameraYSpeed {
    //    get {
    //        return pitch > 0 ? ySpeed * fallMultiplier : ySpeed;
    //    }
    //}

    float clampedYSpeed {
        get {
            return posY < bottom ? Mathf.Max(0, ySpeed) : ySpeed;
        }
    }

    float gravity;
    public float pitch { get; set; }

    #endregion

    #region Methods

    void Start() {
        gravity = baseGravity;
        pitch = transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update () {
        Rotate();
        Move();
        MoveCamera();
    }

    void Rotate() {
        float value = mouseVSpeed * Input.GetAxis("Mouse Y");

        PreventFlyingUp(ref value);

        pitch = invertAxis ? pitch - value : pitch + value;

        ApplyGravity();

        pitch = Mathf.Clamp(pitch, vLimitHigh, vLimitLow);
        //transform.localEulerAngles = Vector3.right * pitch;
    }

    void Move() {
        //Debug.Log(clampedYSpeed);
        //transform.Translate(Vector3.up * clampedYSpeed * Time.deltaTime, Space.World);
    }

    void MoveCamera() {
        //transform.parent.Translate(Vector3.up * cameraYSpeed * Time.deltaTime, Space.World);
    }

    void ApplyGravity() {
        gravity = baseGravity + Mathf.Abs(posY * 20) * gravityModifier;
        pitch += gravity * Time.deltaTime;
    }

    void PreventFlyingUp(ref float value) {
        if (posY > top) {
            if (!isFallingBack) {
                diveModifier = -pitch * fallBackStrength;
                isFallingBack = true;
            }
            value = Mathf.Max(value, 0);
        } else isFallingBack = false;
    }

    #endregion
}
