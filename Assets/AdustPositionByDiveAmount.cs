using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdustPositionByDiveAmount : MonoBehaviour {

    public Vector3 glidePosition;
    public Vector3 dropPosition;

    public float underLerpMultiplier = 0.5f;

    void Start()
    {

        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
    }

    private void GamePlayController_OnDiveAmountChanged(float v, float d)
    {
        if (v < 0)
            v *= underLerpMultiplier;
        transform.localPosition = Vector3.LerpUnclamped(glidePosition, dropPosition, v);
    }
}
