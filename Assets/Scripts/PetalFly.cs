using UnityEngine;
using BASE;
using System;

namespace INTERACT {
    public class PetalFly : MonoBehaviour {
        private SplineWalker walker;
        private GazeObject gaze;
        private ObjectState currState;

        public ParticleSystem glow;
        public ParticleSystem trail;
        public Animator anim;

        public Action FlyEvent;

        private void ResetInteractable() {
            currState = ObjectState.INTERATABLE;
            walker.SetMove(false);
            walker.spline = GameModel.Instance.petalCurvePrefab[(int)GameFlowManager.Instance.currState - 1].GetComponent<BezierSpline>();
            walker.progress = 0;
            glow.Play();
            trail.Stop();

            gaze.GazeEvent += GazeEvent;
            gaze.GazeEnterEvent += GazeEnter;
            gaze.GazeExitEvent += GazeExit;
        }

        private void GazeEnter() {
            anim.SetTrigger("Shake");
            currState = ObjectState.INTERATING;
        }

        private void GazeExit() {
            anim.SetTrigger("Still");
        }

        private void GazeEvent(float x) {
            var emission = glow.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                ExitInteraction();
            }
        }

        private void ExitInteraction() {
            gaze.GazeEvent -= GazeEvent;
            gaze.GazeEnterEvent -= GazeEnter;
            gaze.GazeExitEvent -= GazeExit;
            gaze.focusTime = 0f;

            if (FlyEvent != null) {
                FlyEvent.Invoke();
            }

            anim.SetTrigger("Fly");
            currState = ObjectState.INTERATED;
            glow.Stop();
            walker.SetMove(true);
            walker.onFinish += () => {
                walker.onFinish = null;
                anim.SetTrigger("Still");
                GameFlowManager.Instance.NextState();
            };
            glow.Stop();
            trail.Play();
        }

        private void Start() {
            walker = GetComponent<SplineWalker>();
            gaze = GetComponent<GazeObject>();
            ResetInteractable();
            GameFlowManager.Instance.Register(GameState.COCOON, () => ResetInteractable());
            GameFlowManager.Instance.Register(GameState.FRUIT, () => ResetInteractable());
            GameFlowManager.Instance.Register(GameState.BIRD, () => ResetInteractable());
            GameFlowManager.Instance.Register(GameState.DOG, () => ResetInteractable());
            GameFlowManager.Instance.Register(GameState.PINWHEEL, () => Destroy(gameObject));
        }

    }
}
