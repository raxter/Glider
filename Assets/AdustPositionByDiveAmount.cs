using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdustPositionByDiveAmount : MonoBehaviour {

    public Vector3 glidePosition;
    public Vector3 dropPosition;

    void Start()
    {

        GamePlayController.OnDiveAmountChanged += GamePlayController_OnDiveAmountChanged;
    }

    private void GamePlayController_OnDiveAmountChanged(float v)
    {
        transform.localPosition = Vector3.Lerp(glidePosition, dropPosition, v);
    }
}
