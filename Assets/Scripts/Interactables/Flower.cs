using BASE;
using UnityEngine;

namespace INTERACT {
    public class Flower : MonoBehaviour {
        private GazeBehaviour gaze;

        private void Start() {
            gaze = GetComponent<GazeBehaviour>();
            gaze.GazeEvent += (x) => {
                if (GameFlowManager.Instance.currState == GameState.FLOWIDLE) {
                    GetComponent<Renderer>().material.color = new Color(1, 1, x);
                    if (x == 1) {
                        GameFlowManager.Instance.NextState();
                        // TODO
                        GameFlowManager.Instance.NextState();
                    }
                }
            };
        }
    }
}
