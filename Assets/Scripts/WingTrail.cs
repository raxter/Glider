using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class WingTrail : MonoBehaviour {

    public float angle = 35;

    public Color color = Color.white;

    [SerializeField]
    BirdController birdController;
    //TrailRenderer trail;
    LineRenderer line;
    Color clear;

    public int n = 40;
    
    void Start() {
        //trail = GetComponent<TrailRenderer>();
        line = GetComponent<LineRenderer>();
        clear = color;
        clear.a = 0;
        //trail.endColor = clear;
        //line.endColor = clear;
        line.startColor = clear;

        line.numPositions = n;

        for (int i = 0; i < n; i++)
            line.SetPosition(i, transform.position);
    }
    public float forwardSpeed = 1;
    void Update() {
        //trail.startColor = Color.Lerp(clear, color, birdController.pitch/angle);
        //line.startColor = Color.Lerp(clear, color, birdController.pitch/angle);
        //line.endColor = Color.Lerp(clear, color, birdController.pitch/angle);

        for (int i = 0; i < n-1; i++)
            line.SetPosition(i, line.GetPosition(i+1) + Vector3.back * Time.deltaTime * forwardSpeed);

        line.SetPosition(n-1, transform.position);
    }

}
