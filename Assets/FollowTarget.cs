using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform positionTarget;
    public float dampening = 3;

    // Update is called once per frame
    void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, positionTarget.transform.position, dampening * Time.deltaTime);
        //transform.LookAt(lookAtTarget);
	}
}
