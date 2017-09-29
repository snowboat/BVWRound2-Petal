using BASE;
using UnityEngine;

namespace INTERACT {
    public class DyingFlower : MonoBehaviour {
        private GazeObject gaze;
        public ParticleSystem glow;
        public Animator anim;

        public SpawnObject petal;
        public GameObject dyingPetal;

        private void GazeEvent(float x) {
            var emission = glow.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            anim.Play("Shake", 0, x);
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                ExitInteraction();
            }
        }

        private void ExitInteraction() {
            gaze.GazeEvent -= GazeEvent;
            glow.Stop();
            var p = petal.Spawn();
            GameModel.Instance.petal = p;
            var walker = p.GetComponent<SplineWalker>();
            walker.spline = GameModel.Instance.petalCurvePrefab[0].GetComponent<BezierSpline>();
            walker.SetMove(true);
            walker.onFinish += () => {
                walker.onFinish = null;
                p.GetComponent<GazeObject>().enabled = true;
                p.GetComponent<PetalFly>().enabled = true;
                GameFlowManager.Instance.NextState();
            };
            dyingPetal.SetActive(false);
            anim.enabled = false;
        }

        private void Start() {
            gaze = GetComponent<GazeObject>();
            gaze.GazeEvent += GazeEvent;

            glow.Play();
        }
    }
}
