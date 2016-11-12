using UnityEngine;

public class WingTrail : MonoBehaviour {

    public float angle = 35;

    public Color color = Color.white;

    [SerializeField]
    BirdController birdController;
    TrailRenderer trail;
    Color clear;
    
    void Start() {
        trail = GetComponent<TrailRenderer>();
        clear = color;
        clear.a = 0;
        trail.endColor = clear;
    }

    void Update() {
        trail.startColor = Color.Lerp(clear, color, birdController.pitch/angle);
    }

}
