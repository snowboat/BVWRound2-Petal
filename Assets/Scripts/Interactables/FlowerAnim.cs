using BASE;
using System.Collections;
using UnityEngine;

namespace INTERACT {
    public class FlowerAnim : MonoBehaviour {
        public AudioSource flowerAudio;
        public AudioSource petalAudio;
        private GazeBehaviour gaze;
        public Animator animator;
        public ParticleSystem particle;

        public GameObject[] petals;
        //private float GazeDuration = 0;

        private void Blooming(float x) {
            if (gaze.isFocusing && !flowerAudio.isPlaying) {
                flowerAudio.Play();
            }
            if (!gaze.isFocusing && flowerAudio.isPlaying) {
                flowerAudio.Stop();
            }
            animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 3 * x) * 0.5f;
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                gaze.GazeEvent -= Blooming;
                particle.Stop();
                flowerAudio.Stop();
                gaze.focusTime = 0;
                //Instantiate(GameModel.Instance.grassDistribution);
                Instantiate(GameModel.Instance.flowerDistribution);
            }
        }


        private void Start() {
            gaze = GetComponent<GazeBehaviour>();
            gaze.GazeEvent += Blooming;
            particle.Play();
            //GameFlowManager.Instance.Register(GameState.FLOWIDLE, () => {
            //    gaze.GazeEvent -= Blooming;
            //    particle.Stop();
            //    flowerAudio.Stop();
            //    gaze.focusTime = 0;
            //});
            //GameFlowManager.Instance.Register(GameState.GRASSANIM, () => {
            //    gaze.GazeEvent += PetalFly;
            //    particle.Play();
            //});
            //GameFlowManager.Instance.Register(GameState.PETALIDLE, () => {
            //    for (int i = 0; i < petals.Length; i++) {
            //        petals[i].SetActive(false);
            //    }
            //    gaze.GazeEvent -= PetalFly;
            //    particle.Stop();
            //    StartCoroutine(CheckPetal());
            //    gaze.focusTime = 0;
            //});

            //GameFlowManager.Instance.Register(GameState.MAIN, () => {
            //    gaze.GazeEvent += FlowerExplosion;
            //    particle.Play();
            //});
            //GameFlowManager.Instance.Register(GameState.FINISHFLY, () => {
            //    gaze.GazeEvent -= FlowerExplosion;
            //    particle.Stop();
            //    gaze.focusTime = 0;
            //});
        }
    }
}
