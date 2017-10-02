using BASE;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace INTERACT {
    public class FlowerAnim : MonoBehaviour {
        public AudioSource finalAudio;
        public AudioSource flowerAudio;
        public AudioSource petalAudio;
        private GazeBehaviour gaze;
        public Animator animator;
        public ParticleSystem particle;
        public ParticleSystem creditParticle;
        public AnimationCurve curve;

        public GameObject[] petals;

        public AudioSource narration;
        public AudioClip narrationClip;
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
                GetComponentInChildren<Canvas>().enabled = true;
                creditParticle.Play();
                finalAudio.Play();
                StartCoroutine(StartNarration());
                StartCoroutine(CreditTransition(GetComponentInChildren<Image>()));
                //Instantiate(GameModel.Instance.grassDistribution);
                Instantiate(GameModel.Instance.flowerDistribution);
            }
        }


        private void Start() {
            gaze = GetComponent<GazeBehaviour>();
            gaze.GazeEvent += Blooming;
            particle.Play();
        }

        private IEnumerator CreditTransition(Image image) {
            Color color = Color.white;
            color.a = 0;
            while (image.color.a < 1) {
                color.a += 0.5f * Time.deltaTime;
                image.color = color;
                yield return null;
            }
        }

        private IEnumerator StartNarration() {
            yield return new WaitForSeconds(1f);
            narration.PlayOneShot(narrationClip);
        }
    }
}
