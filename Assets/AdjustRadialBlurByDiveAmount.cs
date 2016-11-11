using UnityEngine;

[RequireComponent(typeof(RadialBlur))]
public class AdjustRadialBlurByDiveAmount : MonoBehaviour {

    public float glideBlur;
    public float dropBlur;

    RadialBlur rb;

    void Start() {
        rb = GetComponent<RadialBlur>();
        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
    }

    private void GamePlayController_OnDiveAmountChanged(float v, float d) {
        rb.blurAmount = Mathf.Lerp(glideBlur, dropBlur, v);
    }
}
