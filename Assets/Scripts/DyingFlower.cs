using BASE;
using UnityEngine;
using System.Collections;

namespace INTERACT {
    public class DyingFlower : MonoBehaviour {
        public AudioSource gazing;
        private GazeObject gaze;
        public ParticleSystem glow;
        public Animator anim;
        public AudioSource audio;
        public AudioClip petalSound;

        public SpawnObject petal;
        public GameObject dyingPetal;

        private void GazeEnter() {
            gazing.Play();
            anim.SetTrigger("Start");
        }

        private void GazeExit() {
            gazing.Stop();
            anim.SetTrigger("Stop");
        }

        private void GazeEvent(float x) {
            var emission = glow.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            //anim.Play("Shake", 0, x);
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                ExitInteraction();
            }
        }

        private void ExitInteraction() {
            gaze.GazeEnterEvent -= GazeEnter;
            gaze.GazeExitEvent -= GazeExit;
            gaze.GazeEvent -= GazeEvent;
            glow.Stop();
            anim.SetTrigger("Stop");
            StartCoroutine(FadeOut(gazing));
            audio.PlayOneShot(petalSound);
            var p = petal.Spawn();
            GameModel.Instance.petal = p;
            var walker = p.GetComponent<SplineWalker>();
            walker.spline = GameModel.Instance.petalCurvePrefab[0].GetComponent<BezierSpline>();
            walker.duration = GameModel.Instance.flyingDuration[0];
            walker.SetMove(true);
            walker.onFinish += () => {
                p.GetComponent<PetalFly>().FlyEvent = null;
                walker.onFinish = null;
                p.GetComponent<GazeObject>().enabled = true;
                p.GetComponent<PetalFly>().enabled = true;
                GameFlowManager.Instance.NextState();
            };
            dyingPetal.SetActive(false);
            anim.enabled = false;
            GameModel.Instance.spawnings[1].Spawn();
        }

        private void Start() {
            audio = GetComponent<AudioSource>();
            gaze = GetComponent<GazeObject>();
            gaze.GazeEvent += GazeEvent;
            gaze.GazeEnterEvent += GazeEnter;
            gaze.GazeExitEvent += GazeExit;

            //glow.Play();
        }

        private IEnumerator FadeOut(AudioSource audio) {
            while (audio.volume > 0) {
                audio.volume -= 0.5f * Time.deltaTime;
                yield return null;
            }
            audio.Stop();
            audio.volume = 1;
        }
    }
}
