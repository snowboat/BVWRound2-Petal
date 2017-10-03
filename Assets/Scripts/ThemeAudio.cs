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
        public AudioSource fountainTheme;
        public float speed;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.CALIBRATE, () => StartCoroutine(FadeIn(petalTheme)));
            GameFlowManager.Instance.Register(GameState.IDLE, () =>
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => StartCoroutine(FadeIn(butterflyTheme))
            );
            GameFlowManager.Instance.Register(GameState.COCOON, () =>
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => StartCoroutine(FadeIn(fountainTheme))
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
            GameFlowManager.Instance.Register(GameState.PINWHEEL, () => {
                StartCoroutine(FadeOut(butterflyTheme));
                StartCoroutine(FadeOut(nestTheme));
                StartCoroutine(FadeOut(dogTheme));
                StartCoroutine(FadeOut(pinwheelTheme));
                StartCoroutine(FadeOut(fountainTheme));
                StartCoroutine(FadeOut(petalTheme));
            });
        }

        private IEnumerator FadeIn(AudioSource audio) {
            while (audio.volume < 1) {
                audio.volume += speed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator FadeOut(AudioSource audio) {
            while (audio.volume >= 0) {
                audio.volume -= speed * Time.deltaTime;
                yield return null;
            }
        }

    }
}
