using UnityEngine;
using System.Collections;
using INTERACT;

namespace BASE {
    public class ThemeAudio : MonoBehaviour {
        public AudioSource petalTheme;
        public AudioSource butterflyTheme;
        public AudioSource nestTheme;
        public AudioSource dogTheme;
        public AudioSource pinwheelTheme;
        public float speed;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.CALIBRATE, () => StartCoroutine(FadeIn(petalTheme)));
            GameFlowManager.Instance.Register(GameState.IDLE, () =>
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => StartCoroutine(FadeIn(butterflyTheme))
            );
            GameFlowManager.Instance.Register(GameState.FRUIT, () =>
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => StartCoroutine(FadeIn(nestTheme))
            );
            GameFlowManager.Instance.Register(GameState.BIRD, () =>
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => StartCoroutine(FadeIn(dogTheme))
            );
            GameFlowManager.Instance.Register(GameState.DOG, () =>
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => StartCoroutine(FadeIn(pinwheelTheme))
            );
        }

        private IEnumerator FadeIn(AudioSource audio) {
            while (audio.volume < 1) {
                audio.volume += speed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
