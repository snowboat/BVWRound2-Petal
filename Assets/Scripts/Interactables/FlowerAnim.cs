using BASE;
using UnityEngine;

namespace INTERACT {
    public class FlowerAnim : MonoBehaviour {
        private GazeBehaviour gaze;
        public Animator animator;

        private void Start() {
            gaze = GetComponent<GazeBehaviour>();
            gaze.GazeEvent += (x) => {
                if (GameFlowManager.Instance.currState == GameState.FLOWIDLE) {
                    animator.Play("Blooming", 0, x);
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
