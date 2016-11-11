using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPoser : MonoBehaviour
{
    [System.Serializable]
    public class Rig
    {
        public Transform head;
        public Transform neck;
        public Transform abdomen;

        public Transform shoulderR;
        public Transform armR;
        public Transform forearmR;
        public Transform wingletR;

        public Transform shoulderL;
        public Transform armL;
        public Transform forearmL;
        public Transform wingletL;

        public Transform tail;

        public Transform tailWingL;
        public Transform tailWingR;

    }

    public Rig rig;

    public float dive;

    public float wingBend;
    public float wingTwist;
    public float flutterFrequency = 10;
    public float flutterScale = 50;

    public float tailBend = 0;

    public float headBend = 0;

    [Range(0, 1)]
    public float flap;
    public float flapFrequency = 5;
    public float flapRange = 50;



    void LateUpdate()
    {
        float flapTwist = 0;
        float flapBend = 0;

        if (flap > 0)
        {
            flapTwist = Mathf.Cos(Time.time * flapFrequency) * flapRange;
            flapBend = Mathf.Sin(Time.time * flapFrequency) * flapRange;
        }

        float twist = Mathf.Lerp(wingTwist, flapTwist, flap);
        float bend = Mathf.Lerp(wingBend, flapBend, flap);

        //rig.armL.Rotate(transform.up * noise);
        // wing bend
        rig.shoulderL.Rotate(transform.right * bend);
        rig.armL.Rotate(transform.right * bend);
        rig.forearmL.Rotate(transform.right * bend);
        rig.wingletL.Rotate(transform.right * bend);
        rig.shoulderR.Rotate(transform.right * bend);
        rig.armR.Rotate(transform.right * bend);
        rig.forearmR.Rotate(transform.right * bend);
        rig.wingletR.Rotate(transform.right * bend);

        // Twist
        rig.shoulderL.Rotate(transform.up * twist);
        rig.armL.Rotate(transform.up * twist);
        rig.forearmL.Rotate(transform.up * twist);
        rig.wingletL.Rotate(transform.up * twist);

        rig.shoulderR.Rotate(-transform.up * twist);
        rig.armR.Rotate(-transform.up * twist);
        rig.forearmR.Rotate(-transform.up * twist);
        rig.wingletR.Rotate(-transform.up * twist);

        // Dive
        rig.armL.Rotate(transform.right * dive);
        rig.forearmL.Rotate(transform.right * -dive * 2);
        rig.wingletL.Rotate(transform.right * dive);

        rig.armR.Rotate(transform.right * dive);
        rig.forearmR.Rotate(transform.right * -dive * 2);
        rig.wingletR.Rotate(transform.right * dive);

        // flutter
        float n1 = (-0.5f + Mathf.PerlinNoise(596.5f, Time.time * flutterFrequency)) * flutterScale;
        float n2 = (-0.5f + Mathf.PerlinNoise(234.2f, Time.time * flutterFrequency)) * flutterScale;

        rig.forearmL.Rotate(transform.right * n1);
        rig.wingletL.Rotate(transform.right * n2);

        rig.forearmR.Rotate(transform.right * n2);
        rig.wingletR.Rotate(transform.right * n1);

        rig.tailWingL.Rotate(transform.right * n1);
        rig.tailWingR.Rotate(transform.right * n2);

        // tail bend
        rig.tail.Rotate(transform.right * tailBend);

        // head bend
        rig.head.Rotate(transform.right * headBend);

    }
}
