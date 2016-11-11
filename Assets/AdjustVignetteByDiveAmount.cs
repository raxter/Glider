using UnityEngine;

[RequireComponent(typeof(Vignette))]
public class AdjustVignetteByDiveAmount : MonoBehaviour {

    public float glideFallOff;
    public float dropFallOff;

    Vignette edge;

    void Start() {
        edge = GetComponent<Vignette>();
        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
    }

    private void GamePlayController_OnDiveAmountChanged(float v) {
        edge.fallOff = Mathf.Lerp(glideFallOff, dropFallOff, v);
    }
}