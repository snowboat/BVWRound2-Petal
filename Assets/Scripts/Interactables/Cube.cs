using UnityEngine;
using BASE;

public class Cube : GazeBehaviour {
    private void Start() {
        GazeEvent += (x) => GetComponent<Renderer>().material.color = new Color(1, 1, x);
    }
}
