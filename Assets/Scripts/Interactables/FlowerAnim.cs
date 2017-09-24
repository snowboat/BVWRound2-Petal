using BASE;
using UnityEngine;

namespace INTERACT {
    public class FlowerAnim : MonoBehaviour {
        private GazeBehaviour gaze;
        public Animator animator;
        public ParticleSystem particle;

        private void Blooming(float x) {
            animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 3 * x) * 0.5f;
            if (x == 1) {
                GameFlowManager.Instance.NextState();
            }
        }

        private void Start() {
            gaze = GetComponent<GazeBehaviour>();
            particle.Play();
            gaze.GazeEvent += Blooming;
            GameFlowManager.Instance.Register(GameState.FLOWIDLE, () => {
                gaze.GazeEvent -= Blooming;
                particle.Stop();
            });
        }
    }
}
