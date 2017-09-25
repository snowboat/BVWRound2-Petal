using BASE;
using UnityEngine;

namespace INTERACT {
    public class FlowerAnim : MonoBehaviour {
        private GazeBehaviour gaze;
        public Animator animator;
        public ParticleSystem particle;
        //private float GazeDuration = 0;

        private void Blooming(float x) {
            animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 3 * x) * 0.5f;
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                GameFlowManager.Instance.NextState();
            }
        }

        private void PetalFly(float x) {
            if (x > 0.2f) {
                GameFlowManager.Instance.NextState();
            }
        }

        private void Start() {
            gaze = GetComponent<GazeBehaviour>();
            gaze.GazeEvent += Blooming;
            particle.Play();
            GameFlowManager.Instance.Register(GameState.FLOWIDLE, () => {
                gaze.GazeEvent -= Blooming;
                particle.Stop();
                gaze.focusTime = 0;
            });
            GameFlowManager.Instance.Register(GameState.GRASSANIM, () => {
                gaze.GazeEvent += PetalFly;
                particle.Play();
            });
            GameFlowManager.Instance.Register(GameState.PETALIDLE, () => {
                gaze.GazeEvent -= PetalFly;
                particle.Stop();
                gaze.focusTime = 0;
            });
        }
    }
}
